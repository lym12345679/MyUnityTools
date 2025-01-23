using System.Collections;
using System.Collections.Generic;
using MizukiTool.AStar;
using UnityEngine;

public class AstarVectorTest : MonoBehaviour, IAstarVectorTarget
{
    public Transform selfTransform;
    public Transform SelfTransform { get => selfTransform; set => selfTransform = value; }
    private float refreshTick = 0;
    public float RefreshTick { get => refreshTick; set => refreshTick = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        ((IAstarVectorTarget)this).CreatAstarVector();
    }
}
