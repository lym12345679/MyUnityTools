using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MizukiTool.AStar;
using System.IO;

public class TestAstarVectorUnit : MonoBehaviour, IAstarVector
{
    public float autoMoveSpeed = 3;
    public float AutoMoveSpeed
    {
        get
        {
            return autoMoveSpeed;
        }
        set
        {
            autoMoveSpeed = value;
        }
    }
    private Transform selfTransform;
    public Transform SelfTransform
    {
        get
        {
            return selfTransform;
        }
        set
        {
            selfTransform = value;
        }
    }

    public Vector3 CurrentDirection { get ; set ; }

    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {
        selfTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        ((IAstarVector)this).AutoMove();
    }
}
