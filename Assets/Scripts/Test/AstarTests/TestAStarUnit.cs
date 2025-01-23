using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MizukiTool.AStar;
public class TestAStarUnit : MonoBehaviour, IAstar
{
    public float speed = 3;
    public Transform target;
    private List<Point> path;
    public List<Point> Path
    {
        get
        {
            return path;
        }
        set
        {
            path = value;
        }
    }
    public Transform TargetTransform
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
    private float pathFindingTick = 0.1f;
    public float PathFindingTick
    {
        get
        {
            return pathFindingTick;
        }
        set
        {
            pathFindingTick = value;
        }
    }
    private float autoMoveSpeed = 3;
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
    public Transform selfTransform;
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


    // Start is called before the first frame update
    void Start()
    {
        if (((IAstar)this).TryFindPath())
        {
            Debug.Log("Find Path");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        ((IAstar)this).AutoMove();
        AstarManager.Instance.path = path;
    }
}
