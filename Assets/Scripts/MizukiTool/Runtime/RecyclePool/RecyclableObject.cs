using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MizukiTool.RecyclePool
{
    /// <summary>
    /// 可回收物体必备组件
    /// </summary>
    public class RecyclableObject : MonoBehaviour
    {
        public float AutoRecycleTime = -1f;
        public UnityEvent OnReset;
        private float recycleTick = 0f;
        public Component MainComponent;

        [HideInInspector]
        public string id;
        private void FixedUpdate()
        {
            if (AutoRecycleTime < 0)
            {
                return;
            }
            recycleTick += Time.fixedDeltaTime;
            if (recycleTick >= AutoRecycleTime)
            {
                //todo:回收该物体
                //Debug.Log("RecycleObject");
                RecyclePool.CollectRecycleObject(this.gameObject, this);
            }
        }
    }

}

