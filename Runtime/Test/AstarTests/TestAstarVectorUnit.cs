using UnityEngine;

namespace MizukiTool.AStar
{
    public class TestAstarVectorUnit : MonoBehaviour, IAstarVector
    {
        public float autoMoveSpeed = 3;

        public Transform targetTransform;

        // Start is called before the first frame update
        private void Start()
        {
            SelfTransform = transform;
        }

        private void FixedUpdate()
        {
            ((IAstarVector)this).AutoMove();
        }

        public float AutoMoveSpeed
        {
            get => autoMoveSpeed;
            set => autoMoveSpeed = value;
        }

        public Transform SelfTransform { get; set; }

        public Vector3 CurrentDirection { get; set; }
    }
}