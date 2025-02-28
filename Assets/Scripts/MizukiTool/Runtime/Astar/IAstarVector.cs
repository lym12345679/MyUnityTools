using UnityEngine;

namespace MizukiTool.AStar
{
    public interface IAstarVector
    {
        public float AutoMoveSpeed { get; set; }
        public Transform SelfTransform { get; set; }
        public Vector3 CurrentDirection { get; set; }

        public Vector3 GetNextDirection()
        {
            return AstarManager.Instance.GetNextDirectionFromAstarVector(SelfTransform.position);
        }


        public void AutoMove()
        {
            var nextDirection = GetNextDirection();
            if (nextDirection == Vector3.zero)
                nextDirection = CurrentDirection;
            else
                CurrentDirection = nextDirection;
            SelfTransform.position += Time.deltaTime * AutoMoveSpeed * nextDirection;
        }
    }
}