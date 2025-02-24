using System;
using UnityEngine;

namespace MizukiTool.Box
{
    public class GeneralBox<T1, T2, T3> : MonoBehaviour where T1 : GeneralBox<T1, T2, T3> where T2 : class where T3 : class
    {
        [HideInInspector]
        public T2 param;

        [HideInInspector]
        public T3 SendParam;
        public readonly string BoxID = Guid.NewGuid().ToString();
        /// <summary>
        /// 召唤UI
        /// </summary>
        /// <param name="param">传入的参数,由子类的T2确定</param>
        /// <returns>创建的GameObject实例</returns>
        public static GameObject Open(T2 param)
        {
            return BoxManager.Instance.OpenBox<T1, T2, T3>(param);
        }
        /// <summary>
        /// 关闭UI
        /// </summary>
        public virtual void Close()
        {
            BoxManager.Instance.CloseBox<T1, T2, T3>(this.BoxID);
        }

        public virtual void GetParams(T2 param)
        {
            this.param = param;
        }
        public virtual T3 SendParams()
        {
            return SendParam;
        }
        public string GetBoxID()
        {
            return BoxID;
        }

    }

}

