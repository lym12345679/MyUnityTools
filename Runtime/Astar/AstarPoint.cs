using UnityEngine;

namespace MizukiTool.AStar
{
    /// <summary>
    ///     地图单一节点的信息
    /// </summary>
    public class Point
    {
        /// <summary>
        ///     向量方向
        /// </summary>
        public Vector3 Direction;

        /// <summary>
        ///     游戏对象
        /// </summary>
        private GameObject gameObject;

        public Component MainCompoment;

        /// <summary>
        ///     是否可行走
        /// </summary>
        public int Mod;

        /// <summary>
        ///     节点价值
        /// </summary>
        public int Value;

        /// <summary>
        ///     记录检测到的墙
        /// </summary>
        public GameObject wall;

        /// <summary>
        ///     坐标值
        /// </summary>
        public int X, Y;

        public Point(int x, int y, int mod, Point parent = null, int value = 1)
        {
            Parent = null;
            X = x;
            Y = y;
            Mod = mod;
            Value = 1;
            Direction = new Vector3(0, 0, 0);
        }

        public Point Parent { get; set; }

        // F G H 值
        //F=G+H
        public float F { get; set; }

        //从起点 A 移动到网格上指定方格的移动耗费 (可沿斜方向移动)
        public float G { get; set; }

        //从指定的方格移动到终点 B 的估算成本，不考虑障碍物
        public float H { get; set; }

        public GameObject GameObject
        {
            get => gameObject;
            set
            {
                if (gameObject == null)
                {
                    gameObject = value;
                    return;
                }

                if ((int)gameObject.transform.position.x != (int)value.transform.position.x)
                    Debug.Log("SetGameObject:" + value.transform.position);
                else if ((int)gameObject.transform.position.y != (int)value.transform.position.y)
                    Debug.Log("SetGameObject:" + value.transform.position);
                gameObject = value;
            }
        }

        public T GetMainCompoment<T>() where T : Component
        {
            if (MainCompoment == null)
                //Debug.Log("(" + X + "," + Y + "):MainCompoment is null");
                return null;
            return MainCompoment as T;
        }

        /// <summary>
        ///     更新G，F 值，和父亲节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="g"></param>
        public void UpdateParent(Point parent, float g)
        {
            Parent = parent;
            G = g;
            F = G + H;
        }
    }
}