using System;
using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.AStar
{
    public class AstarManager : MonoBehaviour
    {
        public static AstarManager Instance;
        public float cellSize = 1;
        public Transform StartTransform;
        public Transform EndTransform;
        public LayerMask WallLayer;
        private readonly int mapHeight = 50;
        private readonly int mapWidth = 50;
        public AstarMap Map;

        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            TryLoadMap(out Map);
            //Test();
        }


        public static void EnsureInstance()
        {
            if (Instance == null)
            {
                var go = new GameObject("AstarManager");
                Instance = go.AddComponent<AstarManager>();
            }
        }

        #region 地图相关

        /// <summary>
        ///     尝试获取地图数据
        /// </summary>
        public bool TryLoadMap(out AstarMap map)
        {
            if (StartTransform != null && EndTransform != null)
            {
                var width = (int)(Mathf.Abs(StartTransform.position.x - EndTransform.position.x) / cellSize);
                var height = (int)(Mathf.Abs(StartTransform.position.y - EndTransform.position.y) / cellSize);
                map = new AstarMap(width, height, cellSize, StartTransform.position);
                map.InitMap();
                map = UpdateMap();
                return true;
            }

            map = new AstarMap(mapWidth, mapHeight, cellSize,
                Vector3.zero - new Vector3(mapWidth * cellSize / 2, mapHeight * cellSize / 2, 0));
            map.InitMap();
            map = UpdateMap();
            Debug.LogWarning("StartTransform or EndTransform is null, use default map data");
            return false;
        }

        /// <summary>
        ///     初始化地图
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        /// <param name="cellSize">单个方块大小</param>
        /// <param name="origin">左下角边缘位置</param>
        /// <param name="mapData">地图数据(以二进制数据0表示该节点不存在该Mod，1表示存在)</param>
        public void InitMap(int width, int height, float cellSize, Vector3 origin, int[,] mapData)
        {
            Map = new AstarMap(width, height, cellSize, origin, mapData);
        }

        /// <summary>
        ///     更新地图信息
        /// </summary>
        public virtual AstarMap UpdateMap()
        {
            var cellSize = Map.GetCellSize();
            var origin = Map.GetOrigin() + new Vector3(cellSize / 2, cellSize / 2, 0);
            var width = Map.GetMapWidth();
            var height = Map.GetMapHeight();
            var mapData = new int[width, height];
            for (var i = 0; i < width; i++)
                //十字检测
                for (var j = 0; j < height; j++)
                {
                    mapData[i, j] = 2;
                    //从左到右发射射线
                    var hit = Physics2D.RaycastAll(origin + new Vector3(i * cellSize - cellSize / 2, j * cellSize, 0),
                        Vector2.right, cellSize, WallLayer);
                    foreach (var h in hit)
                        if (h.collider != null)
                        {
                            mapData[i, j] = 1;
                            break;
                        }

                    //从下到上发射射线
                    hit = Physics2D.RaycastAll(origin + new Vector3(i * cellSize, j * cellSize - cellSize / 2, 0),
                        Vector2.up, cellSize, WallLayer);
                    foreach (var h in hit)
                        if (h.collider != null)
                        {
                            mapData[i, j] = 1;
                            break;
                        }
                }

            Map.SetMapData(mapData);
            return Map;
        }

        #endregion

        #region 路径相关

        [Header("寻路相关")] public float PathFindingDistance = 20;

        public float PathFindingInterval = 0.5f;
        public bool UseSimplePath;

        /// <summary>
        ///     在一定距离外采用直线寻路
        ///     在一定距离内采用A*寻路
        /// </summary>
        /// <param name="startPos">初始位置</param>
        /// <param name="endPos">目标位置</param>
        /// <param name="path">输出路径</param>
        /// <param name="mod">可抵达的节点属性(采用二进制，0为不可抵达，1为可以抵达)</param>
        /// <returns></returns>
        public bool TryFindPath(Vector3 startPos, Vector3 endPos, out List<Point> path, int mod)
        {
            EnsureInstance();
            if (Map == null) TryLoadMap(out Map);
            if (Vector3.Distance(startPos, endPos) > PathFindingDistance)
            {
                path = new List<Point>
                {
                    Map.GetPointOnMap(startPos),
                    Map.GetPointOnMap(endPos)
                };
                return true;
            }

            List<Point> newPath;
            var b = AStarWrapper.TryFindPath(Map, startPos, endPos, out newPath, mod);
            path = newPath;
            return b;
        }

        /// <summary>
        ///     尝试获取路径上的下一个方向
        /// </summary>
        /// <param name="currentPos">当前位置</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public Vector3 NextDirection(Vector3 currentPos, List<Point> path)
        {
            EnsureInstance();
            if (path == null || path.Count == 0) return Vector3.zero;
            var nextPoint = Map.GetPositionOnMap(path[0]);
            //Debug.Log("nextPoint:" + nextPoint);
            return (nextPoint - currentPos).normalized;
        }

        /// <summary>
        ///     简化路径
        /// </summary>
        /// <param name="path">需要简化的路径</param>
        /// <param name="selfPos">自身位置</param>
        /// <returns></returns>
        public virtual List<Point> SimplizePath(List<Point> path, Vector3 selfPos)
        {
            EnsureInstance();
            if (path.Count < 3) return path;
            //Debug.Log("Simplize Path");

            Vector3 startPos;
            Vector3 nextPos;
            var offset = new Vector3(Map.GetCellSize() / 2, Map.GetCellSize() / 2, 0);
            var i = -1;
            while (i < path.Count - 2)
            {
                if (i == -1)
                    startPos = selfPos;
                else
                    startPos = Map.GetPositionOnMap(path[i]) + offset;
                nextPos = Map.GetPositionOnMap(path[i + 1]) + offset;
                var hit1 = Physics2D.Raycast(startPos, nextPos - startPos, Vector3.Distance(startPos, nextPos),
                    WallLayer);
                nextPos = Map.GetPositionOnMap(path[i + 2]) + offset;
                var hit2 = Physics2D.Raycast(startPos, nextPos - startPos, Vector3.Distance(startPos, nextPos),
                    WallLayer);
                if (hit1.collider == null && hit2.collider == null)
                    //Debug.Log("Remove Point:" + path[i + 1]);
                    path.RemoveAt(i + 1);
                else
                    i++;
            }

            return path;
        }

        /// <summary>
        ///     将List<Point>型路径转换为List<Vector3>型
        /// </summary>
        /// <param name="path">需要转换的路径</param>
        /// <param name="selfPos">自身位置</param>
        /// <param name="targetPos">目标位置</param>
        /// <returns></returns>
        public List<Vector3> ConvertPathToVector3(List<Point> path, Vector3 selfPos, Vector3 targetPos)
        {
            var newPath = new List<Vector3>();
            foreach (var p in path) newPath.Add(Map.GetPositionOnMap(p));
            newPath.Insert(0, selfPos);
            newPath.Add(targetPos);
            return newPath;
        }

        /// <summary>
        ///     将List<Vector3>型路径转换为List<Point>型
        /// </summary>
        /// <param name="path">需要转换的路径</param>
        /// <returns></returns>
        public List<Point> ConvertPathToPoint(List<Vector3> path)
        {
            var newPath = new List<Point>();
            foreach (var p in path) newPath.Add(Map.GetPointOnMap(p));
            return newPath;
        }

        #endregion

        #region 向量场寻路相关

        [Header("向量场寻路相关")] public int VectorPathRange = 20;

        public float RefreshInterval = 1f;

        /// <summary>
        ///     创建向量场
        /// </summary>
        /// <param name="targetPos">目标位置</param>
        /// <param name="mod">可抵达的节点属性(采用二进制，0为不可抵达，1为可以抵达)</param>
        public void CreatAstarVector(Vector3 targetPos, int mod)
        {
            EnsureInstance();
            if (Map == null) TryLoadMap(out Map);
            Map = AStarWrapper.CreatAstarVector(Map, targetPos, VectorPathRange, mod);
        }

        /// <summary>
        ///     从向量场中获取下一个方向
        /// </summary>
        /// <param name="currentPos">当前位置</param>
        /// <returns></returns>
        public Vector3 GetNextDirectionFromAstarVector(Vector3 currentPos)
        {
            EnsureInstance();
            if (Map == null) TryLoadMap(out Map);
            var point = Map.GetPointOnMap(currentPos);
            //Debug.Log("NextDirection: " + map[point.X, point.Y].Direction);
            return Map[point.X, point.Y].Direction;
        }

        #endregion

        #region 节点相关

        /// <summary>
        ///     更新CloseList中所有的AstarPoint的Mod
        /// </summary>
        /// <param name="astarMap">用到的地图</param>
        /// <param name="startPos">起始点</param>
        /// <param name="pointMods">可通行的节点Mod</param>
        /// <param name="pointHander">改变Point状态的函数</param>
        /// <returns></returns>
        public AstarMap UpdateAllAstarPonitInCloseList(AstarMap astarMap, Vector3 startPos, int[] pointMods,
            Action<Point> pointHander)
        {
            return AStarWrapper.UpdateAllAstarPonitInCloseList(astarMap, startPos, pointMods, pointHander);
        }

        /// <summary>
        ///     获取相邻的节点
        /// </summary>
        /// <param name="position">当前位置</param>
        /// <returns></returns>
        public List<Point> GetNeighbourPoints(Vector3 position)
        {
            var point = Map.GetPointOnMap(position);
            return AStarWrapper.GetNeighbourPoints(Map, point);
        }

        /// <summary>
        ///     获取相邻的节点
        /// </summary>
        /// <param name="point">当前所在的节点</param>
        /// <returns></returns>
        public List<Point> GetNeighbourPoints(Point point)
        {
            return AStarWrapper.GetNeighbourPoints(Map, point);
        }

        /// <summary>
        ///     获取当前位置所在的节点
        /// </summary>
        /// <param name="position">当前位置</param>
        /// <returns></returns>
        public Point GetPointOnMap(Vector3 position)
        {
            return Map.GetPointOnMap(position);
        }

        #endregion

        #region 标注地图

        [Header("显示地图标注")] public bool ShowMapLableGizmos = true;

        [Header("显示路径标注")] public bool ShowPathLableGizmos = true;

        public Transform ShowPathTarget;
        public List<Point> path;

        private void OnDrawGizmos()
        {
            if (Map != null && ShowMapLableGizmos) DrawMap(Map);
            if (path != null && ShowPathLableGizmos) DrawPath(path, ShowPathTarget);
        }

        protected virtual void DrawMap(AstarMap map)
        {
            var cellSize = map.GetCellSize();
            for (var i = 0; i < map.GetMapWidth(); i++)
                for (var j = 0; j < map.GetMapHeight(); j++)
                {
                    if ((map[i, j].Mod & 1) != 0)
                        Gizmos.color = Color.red;
                    else
                        Gizmos.color = Color.green;
                    var pos = map.GetOrigin() + new Vector3(i * cellSize, j * cellSize, 0);
                    Gizmos.DrawLine(pos + new Vector3(cellSize / 2, 0, 0), pos + new Vector3(cellSize / 2, cellSize, 0));
                    Gizmos.DrawLine(pos + new Vector3(0, cellSize / 2, 0), pos + new Vector3(cellSize, cellSize / 2, 0));
                }
        }

        protected virtual void DrawPath(List<Point> path, Transform selfTransform = null)
        {
            if (path == null || path.Count == 0) return;
            if (selfTransform != null)
            {
                Gizmos.color = Color.blue;
                var startPos = selfTransform.position;
                var endPos = Map.GetPositionOnMap(path[0]);
                Gizmos.DrawLine(startPos, endPos);
            }

            for (var i = 0; i < path.Count - 1; i++)
            {
                Gizmos.color = Color.blue;
                var startPos = Map.GetPositionOnMap(path[i]);
                var endPos = Map.GetPositionOnMap(path[i + 1]);
                Gizmos.DrawLine(startPos, endPos);
            }
        }

        #endregion
    }
}