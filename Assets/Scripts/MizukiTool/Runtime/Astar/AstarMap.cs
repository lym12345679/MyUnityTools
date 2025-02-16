using UnityEngine;
namespace MizukiTool.AStar
{
    /// <summary>
    /// 地图单一节点的信息
    /// </summary>
    public class Point
    {
        public Point Parent { get; set; }
        // F G H 值
        //F=G+H
        public float F { get; set; }

        //从起点 A 移动到网格上指定方格的移动耗费 (可沿斜方向移动)
        public float G { get; set; }

        //从指定的方格移动到终点 B 的估算成本，不考虑障碍物
        public float H { get; set; }

        /// <summary>
        /// 坐标值
        /// </summary>
        public int X, Y;

        /// <summary>
        /// 是否可行走
        /// </summary>
        public int Walkable;
        /// <summary>
        /// 节点价值
        /// </summary>
        public int Value;
        /// <summary>
        /// 记录检测到的墙
        /// </summary>
        public GameObject wall;
        /// <summary>
        /// 向量方向
        /// </summary>
        public Vector3 Direction;
        public Point(int x, int y, int walkable, Point parent = null, int value = 1)
        {
            this.Parent = null;
            this.X = x;
            this.Y = y;
            this.Walkable = walkable;
            this.Value = 1;
            Direction = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// 更新G，F 值，和父亲节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="g"></param>
        public void UpdateParent(Point parent, float g)
        {
            this.Parent = parent;
            this.G = g;
            F = G + H;
        }
    }

    /// <summary>
    /// 由众多节点组成的地图
    /// </summary>
    public class AstarMap
    {
        private Point[,] astarMap = null;
        private int mapHeight, mapWidth;
        float cellSize;
        Vector3 origin = new Vector3(0, 0, 0);
        int[,] mapData;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapHeight">地图高度</param>
        /// <param name="mapWidth">地图宽度</param>
        /// <param name="cellSize">一个正方形节点的大小</param>
        public AstarMap(int mapHeight, int mapWidth, float cellSize = 1)
        {
            astarMap = new Point[mapHeight, mapWidth];
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            this.cellSize = cellSize;
            this.origin = new Vector3(0, 0, 0);
            this.mapData = null;
        }
        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="mapHeight">地图高度/节点数量</param>
        /// <param name="mapWidth">地图宽度/节点数量</param>
        /// <param name="cellSize">一个正方形节点的大小</param>
        /// <param name="origin">初始坐标</param>
        /// <param name="mapData"></param> 
        public AstarMap(int mapHeight, int mapWidth, float cellSize, Vector3 origin, int[,] mapData = null)
        {
            astarMap = new Point[mapHeight, mapWidth];
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            this.cellSize = cellSize;
            this.origin = origin;
            this.mapData = mapData;
        }

        public void InitMap()
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    astarMap[i, j] = new Point(i, j, 0);
                }
            }
        }

        public void InitMap(int[,] mapData)
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }

        public void InitMap(int width, int height, float cellSize, Vector3 origin, int[,] mapData)
        {
            astarMap = new Point[width, height];
            this.mapHeight = width;
            this.mapWidth = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.mapData = mapData;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }
        /// <summary>
        /// 刷新节点
        /// <param name="node">添加的节点信息</param>
        /// </summary>
        public void UpdatePoint(Point node)
        {
            if (node.X >= mapHeight || node.X < 0 || node.Y >= mapWidth || node.Y < 0)
            {
                return;
            }
            astarMap[node.X, node.Y] = node;
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        public Point this[int x, int y]
        {
            get
            {
                if (x >= mapHeight || x < 0 || y >= mapWidth || y < 0)
                {
                    return null;
                }
                return astarMap[x, y];
            }
            set
            {
                astarMap[x, y] = value;
            }
        }
        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Point GetPointOnMap(Vector3 position)
        {
            int x = (int)((position.x - origin.x) / cellSize);
            int y = (int)((position.y - origin.y) / cellSize);
            if (x >= mapHeight || x < 0 || y >= mapWidth || y < 0)
            {
                return null;
            }
            return astarMap[x, y];
        }
        /// <summary>
        /// 获取节点对应地图的位置
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Vector3 GetPositionOnMap(Point point)
        {
            return new Vector3(point.X * cellSize + origin.x, point.Y * cellSize + origin.y, 0);
        }
        public void SetMapData(int[,] mapData)
        {
            this.mapData = mapData;
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }
        public void ResetMapData()
        {
            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWidth; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }
        public int GetMapHeight()
        {
            return mapHeight;
        }
        public int GetMapWidth()
        {
            return mapWidth;
        }
        public float GetCellSize()
        {
            return cellSize;
        }
        public Vector3 GetOrigin()
        {
            return origin;
        }

        
    }


}




