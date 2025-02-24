using System.Collections;
using System.Collections.Generic;
using MizukiTool.RecyclePool;
using UnityEngine;
namespace MizukiTool.Test.TestRecyclePool
{
    public class RecycleTest : MonoBehaviour
    {
        private float Time = 0.1f;

        private float TimeTick = 0;
        // Start is called before the first frame update
        void Start()
        {

        }

        void FixedUpdate()
        {
            TimeTick += UnityEngine.Time.fixedDeltaTime;
            if (TimeTick >= Time)
            {
                TimeTick = 0;
                TestRecyclePoolUtil.Request(TestRecyclePoolEnum.TestRecyclePoolEnum1, (collection) =>
                    {
                        collection.GameObject.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
                    });
            }
        }

    }
    public enum MyTestEnum
    {
        MyTestEnum1
    }
}


