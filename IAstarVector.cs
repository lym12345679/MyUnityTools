using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.AStar
{
    public interface IAstarVector
    {
        public float AutoMoveSpeed { get; set; }
        public Transform SelfTransform { get; set; }
        public Vector3 CurrentDirection { get; set; }
        public Vector3 GetNextDirection() => AstarManager.Instance.GetNextDirection(SelfTransform.position);


        public void AutoMove()
        {
            Vector3 nextDirection = GetNextDirection();
            if (nextDirection == Vector3.zero)
            {
                nextDirection = CurrentDirection;
            }
            else
            {
                CurrentDirection = nextDirection;
            }
            SelfTransform.position += nextDirection * Time.deltaTime * AutoMoveSpeed;
        }
    }
}
