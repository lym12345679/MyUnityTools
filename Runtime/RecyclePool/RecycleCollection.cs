using UnityEngine;

namespace MizukiTool.RecyclePool
{
    //回收物信息
    public class RecycleCollection
    {
        public GameObject GameObject { get; internal set; }
        public Transform Transform { get; internal set; }
        public RecyclableObject RecyclingController { get; internal set; }
        internal Component MainComponent { get; set; }
        public T GetMainComponent<T>() where T : Component
        {
            return (T)MainComponent;
        }
    }
}
