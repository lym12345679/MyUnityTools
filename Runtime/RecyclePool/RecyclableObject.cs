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
        void OnEnable()
        {

        }
        private void FixedUpdate()
        {
            if (AutoRecycleTime < 0)
            {
                return;
            }
            recycleTick += Time.fixedDeltaTime;
            if (recycleTick >= AutoRecycleTime)
            {
                //Debug.Log("RecycleObject");
                CollectRecycleObject(this.gameObject, this);
                recycleTick = 0;
            }
        }
        protected virtual void CollectRecycleObject(GameObject go, RecyclableObject controller)
        {
            RecyclePoolUtil.CollectRecycleObject(go, controller);
        }
    }

}

