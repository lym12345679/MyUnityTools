using System;
using System.Collections.Generic;
using MizukiTool.Test;
using UnityEngine;
namespace MizukiTool.RecyclePool
{

    internal class RecyclePool
    {
        internal Dictionary<string, RecycleContext> contextDic = new Dictionary<string, RecycleContext>();
        internal Dictionary<string, Stack<RecyclableObject>> componentDic = new Dictionary<string, Stack<RecyclableObject>>();

        private EnumIdentifier identifier = new EnumIdentifier();
        private RecycleCollection collection = new RecycleCollection();

        //注册一个回收物
        public void RigisterOnePrefab<T>(T id, GameObject prefab) where T : Enum
        {
            //Debug.Log("RigisterOnePrefab:" + id + ":" + prefab.name);
            identifier.SetEnum(id);
            RecycleContext context = new RecycleContext();
            context.Prefab = prefab;
            context.id = identifier.GetID();
            contextDic.Add(context.id, context);
            componentDic.Add(context.id, new Stack<RecyclableObject>());
        }
        //检查是否存在该回收物
        public bool CheckIdentifer<T>(T id) where T : Enum
        {
            identifier.SetEnum(id);
            if (contextDic.TryGetValue(identifier.GetID(), out RecycleContext context))
            {
                return true;
            }
            Debug.LogError("RecyclePool:No such RecycleObject:" + id + ",注册请转:MizukiTool.RecyclePoolUtil.RigisterAllPrefab");
            return false;
        }
        //创建一个回收物
        public GameObject Create<T>(T id) where T : Enum
        {
            identifier.SetEnum<T>(id);
            RecycleContext context = contextDic[identifier.GetID()];
            GameObject go = GameObject.Instantiate(context.Prefab);
            RecyclableObject controller;
            if (go.TryGetComponent<RecyclableObject>(out controller))
            {
                controller.id = identifier.GetID();
            }
            else
            {
                controller = go.AddComponent<RecyclableObject>();
                controller.id = identifier.GetID();
            }
            return go;
        }

        //请求一个回收物
        public void Request<T>(T id, Action<RecycleCollection> hander = null, Transform parent = null) where T : Enum
        {
            //Debug.Log("Request");
            GameObject target;
            collection = new RecycleCollection();
            RecyclableObject controller;
            if (CheckIdentifer(id))
            {
                identifier.SetEnum(id);
                if (componentDic.TryGetValue(identifier.GetID(), out Stack<RecyclableObject> stack))
                {
                    while (stack.Count > 0 && stack.Peek() == null)
                    {
                        controller = stack.Pop();
                    }
                    if (stack.Count == 0)
                    {
                        target = Create(id);
                        controller = target.GetComponent<RecyclableObject>();
                    }
                    else
                    {
                        controller = stack.Pop();
                    }
                    target = controller.gameObject;
                    target.gameObject.SetActive(true);
                    controller.OnReset.Invoke();
                    collection.GameObject = target;
                    collection.RecyclingController = controller;
                    collection.MainComponent = controller.MainComponent;
                    if (parent != null)
                    {
                        target.transform.SetParent(parent);
                    }
                    else
                    {
                        target.transform.SetParent(null);
                    }
                    if (hander != null)
                    {
                        hander.Invoke(collection);
                    }
                }

            }
        }
        //回收一个回收物
        public void CollectRecycleObject(GameObject go, RecyclableObject controller)
        {
            EnsureSceneRecycleGuardExist();
            go.SetActive(false);
            if (!componentDic.ContainsKey(controller.id))
            {
                Debug.LogError("RecyclePool:对象的预制体未注册:" + controller.id + "默认删除,注册请转:MizukiTool.RecyclePoolUtil.RigisterAllPrefab");
                GameObject.Destroy(go);
                return;
            }
            componentDic[controller.id].Push(controller);
            go.transform.SetParent(SceneRecycleGuard.Instance.transform);
        }
        public void ReturnToPool(GameObject go)
        {
            RecyclableObject controller;
            if (go.TryGetComponent<RecyclableObject>(out controller))
            {
                CollectRecycleObject(go, controller);
            }
            else
            {
                Debug.LogError("RecyclePool:对象不存在组件RecyclableObject:" + go.name + "默认删除");
                GameObject.Destroy(go);
            }
        }
        #region 确认是否存在        

        public void EnsureSceneRecycleGuardExist()
        {
            if (SceneRecycleGuard.Instance == null)
            {
                GameObject go = new GameObject("SceneRecyclePool");
                go.AddComponent<SceneRecycleGuard>();
            }
        }
        #endregion
    }

}