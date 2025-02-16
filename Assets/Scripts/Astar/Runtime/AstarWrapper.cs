using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.AStar
{
    /// <summary>
    /// A*算法的封装
    /// </summary> 
    public static class AStarWrapper
    {

        public static List<Point> path = new List<Point>();
        private static List<Point> openList = new List<Point>();
        private static List<Point> closeList = new List<Point>();
        //针对一个NPC的寻路
        public static bool TryFindPath(AstarMap astarMap, Vector3 startPos, Vector3 endPos, out List<Point> outPath)
        {
            Point start = astarMap.GetPointOnMap(startPos);
            Point end = astarMap.GetPointOnMap(endPos);
            path.Clear();
            openList.Clear();
            closeList.Clear();
            openList.Add(start);
            while (openList.Count > 0)
            {
                //找到openList中F值最小的节点
                Point point = MinFOfOpenlist(openList);

                //把该节点从openList中移除，之后移入closeList中
                openList.Remove(point);
                closeList.Add(point);

                //找到该节点的相邻节点
                List<Point> surroundPoint = FindSurroundPoint(astarMap, point);
                //过滤closeList中的节点,并且更新openList中的节点
                UpdateCloseListAndOpenList(surroundPoint, closeList, openList, point);
            }
            if (closeList.Contains(end))
            {
                Point temp = end;
                while (temp != start)
                {
                    path.Add(temp);
                    temp = temp.Parent;
                }
                path.Reverse();
                outPath = path;
                return true;
            }
            outPath = null;
            return false;
        }
        //向量场寻路
        public static AstarMap CreatAstarVector(AstarMap astarMap, Vector3 startPos, int range)
        {
            Point start = astarMap.GetPointOnMap(startPos);
            openList.Clear();
            closeList.Clear();
            openList.Add(start);
            int i = 0;
            Point flag = start;
            while (openList.Count > 0 && i < range)
            {
                //找到openList第一个点
                Point point = openList[0];
                if (flag == point)
                {
                    flag = openList[openList.Count - 1];
                    i++;
                }
                //把该节点从openList中移除，之后移入closeList中
                openList.Remove(point);
                closeList.Add(point);

                //找到该节点的相邻节点
                List<Point> surroundPoint = FindSurroundPoint(astarMap, point);
                //过滤closeList中的节点,并且更新openList中的节点
                UpdateCloseListAndOpenList(surroundPoint, closeList, openList, point);
            }
            foreach (var point in closeList)
            {
                if (point.Parent == null)
                {
                    continue;
                }
                point.Direction = new Vector3(point.Parent.X - point.X, point.Parent.Y - point.Y, 0);
                point.Direction.Normalize();
                astarMap[point.X, point.Y].Direction = point.Direction;
                //Debug.Log("SetDirection: " + astarMap[point.X, point.Y].Direction);
            }
            return astarMap;
        }

        private static Point MinFOfOpenlist(List<Point> openList)
        {
            Point minPoint = openList[0];
            foreach (var p in openList)
            {
                if (p.F < minPoint.F)
                {
                    minPoint = p;
                }
            }
            return minPoint;
        }

        private static List<Point> FindSurroundPoint(AstarMap map, Point point)
        {
            List<Point> surroundPoints = new List<Point>();
            Point up = null, down = null, left = null, right = null;
            Point upLeft = null, upRight = null, downLeft = null, downRight = null;
            int x = point.X;
            int y = point.Y;
            if (map[x, y + 1] != null)
            {
                right = map[x, y + 1];
                if (right.Walkable == 0)
                {
                    surroundPoints.Add(right);
                }
            }
            if (map[x, y - 1] != null)
            {
                left = map[x, y - 1];
                if (left.Walkable == 0)
                {
                    surroundPoints.Add(left);
                }
            }
            if (map[x + 1, y] != null)
            {
                up = map[x + 1, y];
                if (up.Walkable == 0)
                {
                    surroundPoints.Add(up);
                }
            }
            if (map[x - 1, y] != null)
            {
                down = map[x - 1, y];
                if (down.Walkable == 0)
                {
                    surroundPoints.Add(down);
                }
            }
            if (map[x + 1, y + 1] != null && right != null && up != null)
            {
                upRight = map[x + 1, y + 1];
                if (upRight.Walkable == 0 && (right.Walkable == 0 || up.Walkable == 0))
                {
                    surroundPoints.Add(upRight);
                }
            }
            if (map[x + 1, y - 1] != null && left != null && up != null)
            {
                upLeft = map[x + 1, y - 1];
                if (upLeft.Walkable == 0 && (left.Walkable == 0 || up.Walkable == 0))
                {
                    surroundPoints.Add(upLeft);
                }
            }
            if (map[x - 1, y + 1] != null && right != null && down != null)
            {
                downRight = map[x - 1, y + 1];
                if (downRight.Walkable == 0 && (right.Walkable == 0 || down.Walkable == 0))
                {
                    surroundPoints.Add(downRight);
                }
            }
            if (map[x - 1, y - 1] != null && left != null && down != null)
            {
                downLeft = map[x - 1, y - 1];
                if (downLeft.Walkable == 0 && (left.Walkable == 0 || down.Walkable == 0))
                {
                    surroundPoints.Add(downLeft);
                }
            }
            return surroundPoints;
        }

        private static void UpdateCloseListAndOpenList(List<Point> surroundPoints, List<Point> closeList, List<Point> openList, Point currentPoint)
        {
            //过滤closeList中的节点,并且更新openList中的节点
            foreach (var point in surroundPoints)
            {
                if (closeList.Contains(point))
                {
                    continue;
                }

                if (!openList.Contains(point))
                {
                    point.UpdateParent(currentPoint, point.G);
                    openList.Add(point);
                }
                else
                {
                    float newG = point.Parent.G + 1;
                    if (newG < point.G)
                    {
                        point.UpdateParent(currentPoint, newG);
                    }
                }
            }
        }

    }
}