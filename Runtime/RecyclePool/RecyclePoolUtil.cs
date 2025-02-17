using System;
using UnityEngine;

namespace MizukiTool.RecyclePool
{
    public class RecyclePoolUtil : RecyclePool
    {
        public static new void Request<T>(T id, Action<RecycleCollection> hander = null, Transform parent = null) where T : Enum
             => RecyclePool.Request(id, hander, parent);

        public static new void ReturnToPool(GameObject go)
            => RecyclePool.ReturnToPool(go);

        public static override void RigisterAllPrefab()
        {
            //RigisterOnePrefab(TextShowEnum.Item1, Resources.Load<GameObject>("Prefeb/UIPrefeb/TextShow/TextShowItem"));
        }
    }
}

