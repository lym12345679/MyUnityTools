using UnityEngine;

namespace MizukiTool.AStar
{
    /// <summary>
    ///     由众多节点组成的地图
    /// </summary>
    public class AstarMap
    {
        private Point[,] astarMap;
        private float cellSize;
        private int[,] mapData;
        private int mapHeight, mapWidth;
        private Vector3 origin = new(0, 0, 0);

        /// <summary>
        ///     构造函数
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
            origin = new Vector3(0, 0, 0);
            mapData = null;
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="mapHeight">地图高度/节点数量</param>
        /// <param name="mapWidth">地图宽度/节点数量</param>
        /// <param name="cellSize">一个正方形节点的大小</param>
        /// <param name="origin">初始坐标</param>
        /// <param name="mapData"></param>
        public AstarMap(int mapWidth, int mapHeight, float cellSize, Vector3 origin, int[,] mapData = null)
        {
            astarMap = new Point[mapWidth, mapHeight];
            this.mapHeight = mapHeight;
            this.mapWidth = mapWidth;
            this.cellSize = cellSize;
            this.origin = origin;
            this.mapData = mapData;
        }

        /// <summary>
        ///     获取节点
        /// </summary>
        public Point this[int x, int y]
        {
            get
            {
                if (y >= mapHeight || y < 0 || x >= mapWidth || x < 0) return null;
                return astarMap[x, y];
            }
            set => astarMap[x, y] = value;
        }

        public void InitMap()
        {
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                astarMap[i, j] = new Point(i, j, 0);
        }

        public void InitMap(int[,] mapData)
        {
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                astarMap[i, j] = new Point(i, j, mapData[i, j]);
        }

        public void InitMap(int width, int height, float cellSize, Vector3 origin, int[,] mapData)
        {
            astarMap = new Point[width, height];
            mapHeight = width;
            mapWidth = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.mapData = mapData;
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                astarMap[i, j] = new Point(i, j, mapData[i, j]);
        }

        /// <summary>
        ///     刷新节点
        ///     <param name="node">添加的节点信息</param>
        /// </summary>
        public void UpdatePoint(Point node)
        {
            if (node.Y >= mapHeight || node.Y < 0 || node.X >= mapWidth || node.X < 0) return;
            astarMap[node.X, node.Y] = node;
        }

        /// <summary>
        ///     获取节点
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Point GetPointOnMap(Vector3 position)
        {
            var x = (int)((position.x - origin.x) / cellSize);
            var y = (int)((position.y - origin.y) / cellSize);
            if (y >= mapHeight || y < 0 || x >= mapWidth || x < 0) return null;
            //Debug.Log("GetPointOnMap:(" + x + "," + y + ")");
            return astarMap[x, y];
        }

        /// <summary>
        ///     获取节点对应地图的位置
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
            Debug.Log("SetMapData:(" + mapWidth + "," + mapHeight + " )(" + mapData.Length + ")");
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                //Debug.Log("SetMapData: (" + i + "," + j + ")" + mapData[i, j]);
                astarMap[i, j] = new Point(i, j, mapData[i, j]);
        }

        public void ResetMapData()
        {
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                astarMap[i, j] = new Point(i, j, mapData[i, j]);
        }

        public void SetGameObjects(GameObject[,] gameObjects)
        {
            //Debug.Log("SetGameObjects:(" + mapWidth + "," + mapHeight + " )(" + gameObjects.Length + ")");
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                /*if (gameObjects[i, j] != null)
                    {
                        Debug.Log("SetGameObjects: (" + j + "," + i + ")" + gameObjects[i, j].transform.position);
                    }*/
                astarMap[i, j].GameObject = gameObjects[i, j];
        }

        public void SetMainCompoment(Component[,] mainCompoments)
        {
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                astarMap[i, j].MainCompoment = mainCompoments[i, j];
        }

        public void ResetGameObjects()
        {
            for (var i = 0; i < mapWidth; i++)
            for (var j = 0; j < mapHeight; j++)
                astarMap[i, j].GameObject = null;
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