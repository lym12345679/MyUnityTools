using System;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.RecyclePool
{

    internal class RecyclePool
    {
        internal static Dictionary<string, RecycleContext> contextDic = new Dictionary<string, RecycleContext>();
        internal static Dictionary<string, Stack<RecyclableObject>> componentDic = new Dictionary<string, Stack<RecyclableObject>>();
        private static bool isPrefabRegistered = false;
        private static EnumIdentifier identifier = new EnumIdentifier();
        private static RecycleCollection collection = new RecycleCollection();
        //注册所有回收物
        //格式:RigisterOnePrefab(Enum,GameObject)
        public virtual static void RigisterAllPrefab()
        {
            //RigisterOnePrefab(TextShowEnum.Item1, Resources.Load<GameObject>("Prefeb/UIPrefeb/TextShow/TextShowItem"));
        }
        //注册一个回收物
        public static void RigisterOnePrefab<T>(T id, GameObject prefab) where T : Enum
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
        public static bool CheckIdentifer<T>(T id) where T : Enum
        {
            identifier.SetEnum(id);
            if (contextDic.TryGetValue(identifier.GetID(), out RecycleContext context))
            {
                return true;
            }
            Debug.LogError("RecyclePool:No such RecycleObject:" + id + ",注册请转:MizukiTool.RecyclePool.RigisterAllPrefab");
            return false;
        }
        //创建一个回收物
        public static GameObject Create<T>(T id) where T : Enum
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
        public static void Request<T>(T id, Action<RecycleCollection> hander = null, Transform parent = null) where T : Enum
        {
            //Debug.Log("Request");
            EnsureContextExist();
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
                    collection.RecyclingController = controller;
                    collection.MainComponent = controller.MainComponent;
                    if (parent != null)
                    {
                        target.transform.SetParent(parent);
                    }
                    if (hander != null)
                    {
                        hander.Invoke(collection);
                    }
                }

            }
        }
        //回收一个回收物
        public static void CollectRecycleObject(GameObject go, RecyclableObject controller)
        {
            EnsureSceneRecycleGuardExist();
            go.SetActive(false);
            if (!componentDic.ContainsKey(controller.id))
            {
                Debug.LogError("RecyclePool:对象的预制体未注册:" + controller.id + "默认删除,注册请转:MizukiTool.RecyclePool.RigisterAllPrefab");
                GameObject.Destroy(go);
                return;
            }
            componentDic[controller.id].Push(controller);
            go.transform.SetParent(SceneRecycleGuard.Instance.transform);
        }
        public static void ReturnToPool(GameObject go)
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
        public static void EnsureContextExist()
        {
            if (isPrefabRegistered)
            {
                return;
            }
            isPrefabRegistered = true;
            RigisterAllPrefab();
        }
        public static void EnsureSceneRecycleGuardExist()
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