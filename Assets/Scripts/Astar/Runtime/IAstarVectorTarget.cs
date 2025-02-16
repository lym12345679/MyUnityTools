using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.AStar
{
    public interface IAstarVectorTarget
    {
        public Transform SelfTransform { get; set; }
        public float RefreshTick { get; set; }
        public void CreatAstarVector()
        {
            if (RefreshTick < AstarManager.Instance.RefreshInterval)
            {
                RefreshTick += Time.deltaTime;
                return;
            }
            Debug.Log("CreatAstarVector");
            AstarManager.Instance.CreatAstarVector(SelfTransform.position);
            RefreshTick = 0;
        }

    }
}

