using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.Box
{
    public class BoxManager
    {
        private static BoxManager instance;
        public static BoxManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BoxManager();
                }
                return instance;
            }
        }
        private Dictionary<string, GameObject> boxDic = new Dictionary<string, GameObject>();
        /// <summary>
        /// 打开UI，UI必须挂上T1类的脚本
        /// </summary>
        /// <param name="boxEnum">UI枚举</param>
        /// <param name="param">参数类型</param>
        public GameObject OpenBox<T1, T2, T3>(T2 param) where T1 : GeneralBox<T1, T2, T3> where T2 : class where T3 : class
        {
            string boxEnum = GetBoxTypeDict()[typeof(T1)];
            //根据枚举类型获取路径
            string path = GetBoxPathDict()[boxEnum];
            //加载预制体
            GameObject go = Resources.Load<GameObject>(path);
            //实例化预制体
            GameObject target = GameObject.Instantiate(go);
            target.name = target.name.Replace("(Clone)", "");
            //获取预制体上的T1类的脚本
            T1 t1 = target.GetComponent<T1>();
            //传入参数
            t1.GetParams(param);
            string boxID = t1.GetBoxID();
            //注册预制体
            RegisterBox(boxID, target);
            return target;
        }

        private void RegisterBox(string boxID, GameObject go)
        {
            while (!boxDic.TryAdd(boxID, go)) ;
        }

        /// <summary>
        /// 关闭UI
        /// </summary>
        /// <param name="boxID">UI ID</param>
        public void CloseBox<T1, T2, T3>(string boxID) where T1 : GeneralBox<T1, T2, T3> where T2 : class where T3 : class
        {
            if (boxDic.ContainsKey(boxID))
            {
                GameObject.Destroy(boxDic[boxID]);
                UnRegisterBox(boxID);
            }
        }

        public void UnRegisterBox(string boxID)
        {
            if (boxDic.ContainsKey(boxID))
            {
                boxDic.Remove(boxID);
            }
        }

        protected virtual Dictionary<System.Type, string> GetBoxTypeDict()
        {
            return BoxDict.BoxTypeDic;
        }

        protected virtual Dictionary<string, string> GetBoxPathDict()
        {
            return BoxDict.BoxPathDic;
        }
    }
}

