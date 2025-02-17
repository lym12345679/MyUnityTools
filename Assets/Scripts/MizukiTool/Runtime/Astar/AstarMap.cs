using System;
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
        public PointMod Mod;
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
        /// <summary>
        /// 游戏对象
        /// </summary>
        private GameObject gameObject;
        public Component MainCompoment;
        public T GetMainCompoment<T>() where T : Component
        {
            if (MainCompoment == null)
            {
                //Debug.Log("(" + X + "," + Y + "):MainCompoment is null");
                return null;
            }
            return MainCompoment as T;
        }
        public GameObject GameObject
        {
            get { return gameObject; }
            set
            {
                if (gameObject == null)
                {
                    gameObject = value;
                    return;
                }
                if ((int)gameObject.transform.position.x != (int)value.transform.position.x)
                {
                    Debug.Log("SetGameObject:" + value.transform.position);
                }
                else if ((int)gameObject.transform.position.y != (int)value.transform.position.y)
                {
                    Debug.Log("SetGameObject:" + value.transform.position);
                }
                gameObject = value;
            }
        }
        public Point(int x, int y, PointMod walkable, Point parent = null, int value = 1)
        {
            this.Parent = null;
            this.X = x;
            this.Y = y;
            this.Mod = walkable;
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
        PointMod[,] mapData;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mapHeight">地图高度</param>
        /// <param name="mapWidth">地图宽度</param>
        /// <param name="cellSize">一个正方形节点的大小</param>
        public AstarMap(int mapWidth, int mapHeight, float cellSize = 1)
        {
            astarMap = new Point[mapWidth, mapHeight];
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
        public AstarMap(int mapWidth, int mapHeight, float cellSize, Vector3 origin, PointMod[,] mapData = null)
        {
            astarMap = new Point[mapWidth, mapHeight];
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            this.cellSize = cellSize;
            this.origin = origin;
            this.mapData = mapData;
        }

        public void InitMap()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    astarMap[i, j] = new Point(i, j, 0);
                }
            }
        }

        public void InitMap(PointMod[,] mapData)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }

        public void InitMap(int width, int height, float cellSize, Vector3 origin, PointMod[,] mapData)
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
            if (node.Y >= mapHeight || node.Y < 0 || node.X >= mapWidth || node.X < 0)
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
                if (y >= mapHeight || y < 0 || x >= mapWidth || x < 0)
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
            if (y >= mapHeight || y < 0 || x >= mapWidth || x < 0)
            {
                return null;
            }
            //Debug.Log("GetPointOnMap:(" + x + "," + y + ")");
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
        public void SetMapData(PointMod[,] mapData)
        {
            this.mapData = mapData;
            Debug.Log("SetMapData:(" + this.mapWidth + "," + this.mapHeight + " )(" + mapData.Length + ")");
            for (int i = 0; i < this.mapWidth; i++)
            {
                for (int j = 0; j < this.mapHeight; j++)
                {
                    //Debug.Log("SetMapData: (" + i + "," + j + ")" + mapData[i, j]);
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }
        public void ResetMapData()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    astarMap[i, j] = new Point(i, j, mapData[i, j]);
                }
            }
        }
        public void SetGameObjects(GameObject[,] gameObjects)
        {
            //Debug.Log("SetGameObjects:(" + mapWidth + "," + mapHeight + " )(" + gameObjects.Length + ")");
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    /*if (gameObjects[i, j] != null)
                    {
                        Debug.Log("SetGameObjects: (" + j + "," + i + ")" + gameObjects[i, j].transform.position);
                    }*/
                    astarMap[i, j].GameObject = gameObjects[i, j];
                }
            }
        }
        public void SetMainCompoment(Component[,] mainCompoments)
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    astarMap[i, j].MainCompoment = mainCompoments[i, j];
                }
            }
        }
        public void ResetGameObjects()
        {
            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    astarMap[i, j].GameObject = null;
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




