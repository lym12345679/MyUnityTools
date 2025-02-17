using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.AStar
{
    public class AstarVectorTest : MonoBehaviour, IAstarVectorTarget
    {
        public Transform selfTransform;
        public Transform SelfTransform { get => selfTransform; set => selfTransform = value; }
        private float refreshTick = 0;
        public float RefreshTick { get => refreshTick; set => refreshTick = value; }
        void FixedUpdate()
        {
            ((IAstarVectorTarget)this).CreatAstarVector();
        }
    }

}
