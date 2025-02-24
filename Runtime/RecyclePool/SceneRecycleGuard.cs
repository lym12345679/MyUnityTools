using UnityEngine;

namespace MizukiTool.RecyclePool
{
    //地图可回收物集中点
    internal class SceneRecycleGuard : MonoBehaviour
    {
        public static SceneRecycleGuard Instance;
        void Awake()
        {
            Instance = this;
        }
    }
}