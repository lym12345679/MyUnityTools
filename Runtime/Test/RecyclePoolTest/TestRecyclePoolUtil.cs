using System;
using UnityEngine;
using MizukiTool.RecyclePool;
namespace MizukiTool.Test.TestRecyclePool
{
    public static class TestRecyclePoolUtil
    {
        private static bool isRigister = false;
        /// <summary>
        /// 在对象池中请求物体
        /// </summary>
        /// <param name="id">对象池枚举(可以是任何一个)</param>
        /// <param name="hander">对象处理方式</param>
        /// <param name="parent">设置父物体</param>
        /// <typeparam name="T">枚举</typeparam>
        public static void Request<T>(T id, Action<RecycleCollection> hander = null, Transform parent = null) where T : Enum
        {
            if (!isRigister)
            {
                isRigister = true;
                SetRigisterAction();
            }
            RecyclePoolUtil.Request(id, hander, parent);
        }

        /// <summary>
        /// 将从对象池请求的物体返回对象池
        /// </summary>
        /// <param name="go">需要回收的物体</param>
        public static void ReturnToPool(GameObject go)
            => RecyclePoolUtil.ReturnToPool(go);

        /// <summary>
        /// 在这里注册所有对象
        /// 参考格式:recyclePool.RigisterOnePrefab(TargetEnum, TargetPrefab);
        // etc:recyclePool.RigisterOnePrefab(MyTestEnum.MyTestEnum1, Resources.Load<GameObject>("Prefab/Recycle/RecycleGO"));
        /// </summary>
        public static void SetRigisterAction()
        {
            Action<RecyclePool.RecyclePool> action = (recyclePool) =>
            {
                recyclePool.RigisterOnePrefab(TestRecyclePoolEnum.TestRecyclePoolEnum1, Resources.Load<GameObject>("Prefab/Test/Recycle/TestRecycleGO"));
            };
            RecyclePoolUtil.SetRigisterAction(action);
        }

    }

    public enum TestRecyclePoolEnum
    {
        TestRecyclePoolEnum1,
    }
}

