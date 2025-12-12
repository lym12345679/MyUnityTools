using System;
using UnityEngine;

namespace MizukiTool.RecyclePool
{
    public static class RecyclePoolUtil
    {
        private static bool isPrefabRegistered;
        private static readonly RecyclePool recyclePool = new();
        private static Action<RecyclePool> rigisterAction;

        /// <summary>
        ///     在对象池中请求物体
        /// </summary>
        /// <param name="id">对象池枚举(可以是任何一个)</param>
        /// <param name="hander">对象处理方式</param>
        /// <param name="parent">设置父物体</param>
        /// <typeparam name="T">枚举</typeparam>
        public static void Request<T>(T id, Action<RecycleCollection> hander = null, Transform parent = null)
            where T : Enum
        {
            EnsureContextExist();
            recyclePool.Request(id, hander, parent);
        }

        /// <summary>
        ///     将从对象池请求的物体返回对象池
        /// </summary>
        /// <param name="go">需要回收的物体</param>
        public static void ReturnToPool(GameObject go)
        {
            recyclePool.ReturnToPool(go);
        }

        internal static void CollectRecycleObject(GameObject go, RecyclableObject controller)
        {
            recyclePool.CollectRecycleObject(go, controller);
        }

        private static void EnsureContextExist()
        {
            if (isPrefabRegistered) return;
            RigisterAllPrefab();
        }

        public static void SetRigisterAction(Action<RecyclePool> action)
        {
            rigisterAction = action;
        }

        /// <summary>
        ///     在这里注册所有对象
        ///     参考格式:recyclePool.RigisterOnePrefab(TargetEnum, TargetPrefab);
        ///     etc:recyclePool.RigisterOnePrefab(MyTestEnum.MyTestEnum1, Resources.Load<GameObject>("Prefab/Recycle/RecycleGO"));
        /// </summary>
        public static void RigisterAllPrefab()
        {
            isPrefabRegistered = true;
            rigisterAction?.Invoke(recyclePool);
        }
    }
}