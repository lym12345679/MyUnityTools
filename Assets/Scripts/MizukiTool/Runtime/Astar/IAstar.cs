using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
namespace MizukiTool.AStar
{
    public interface IAstar
    {
        public List<Point> Path { get; set; }
        public Transform SelfTransform { get; set; }
        public Transform TargetTransform { get; set; }
        public float AutoMoveSpeed { get; set; }
        public float PathFindingTick { get; set; }
        /// <summary>
        /// 尝试寻找路径
        /// </summary>
        /// <param name="start">起始点</param>
        /// <param name="end">终点</param>
        /// <param name="path">保存的路径</param>
        /// <returns>是否能够寻找到</returns>
        public bool TryFindPath()
        {
            List<Point> newPath = new List<Point>();
            if (AstarManager.Instance.TryFindPath(SelfTransform.position, TargetTransform.position, out newPath))
            {
                Path = newPath;
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 下一个方向
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="currentPos">当前位置</param>
        public Vector3 NextDirection() => AstarManager.Instance.NextDirection(SelfTransform.position, Path);

        /// <summary>
        /// 自动移动,放在fixedUpdate中
        /// </summary>
        public void AutoMove()
        {
            AstarMap map = AstarManager.Instance.map;
            if (Path == null || Path.Count == 0)
            {
                return;
            }
            Vector3 nextDirection = NextDirection();
            SelfTransform.position += nextDirection * Time.deltaTime * AutoMoveSpeed;
            if (Vector3.Distance(SelfTransform.position, map.GetPositionOnMap(Path[0])) < 0.1)
            {
                //Debug.Log("Arrive at next point");
                Path.RemoveAt(0);
            }
            PathFindingTick += Time.deltaTime;
            if (PathFindingTick > AstarManager.Instance.PathFindingInterval)
            {
                PathFindingTick = 0;
                if (TryFindPath())
                {
                    //Debug.Log("Find Path");
                    if (AstarManager.Instance.UseSimplePath)
                        Path = AstarManager.Instance.SimplizePath(Path, SelfTransform.position, TargetTransform.position);
                }
            }

        }


    }
}


