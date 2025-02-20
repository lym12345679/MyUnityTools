using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.MiAudio
{
    public class GameObjectPool
    {
        GameObject mRoot;
        GameObject mEnabled;
        GameObject mDisabled;
        int mTotal;
        Stack<GameObject> mGOStack = new Stack<GameObject>();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="root"></param>
        public void Init(GameObject root)
        {
            mRoot = new GameObject("GOPool");
            mRoot.transform.parent = root.transform;
            mEnabled = new GameObject("Enabled");
            mEnabled.transform.parent = mRoot.transform;
            mDisabled = new GameObject("Disabled");
            mDisabled.transform.parent = mRoot.transform;
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            if (mGOStack.Count == 0)
            {
                mTotal++;
                GameObject go = new GameObject("GOPool:" + mTotal.ToString());
                AudioManager.DontDestroyOnLoad(go);
                return go;
            }
            GameObject target = mGOStack.Pop();
            target.SetActive(true);
            return target;
        }
        /// <summary>
        /// 归还对象
        /// </summary>
        /// <param name="go"></param>
        public void Free(GameObject go)
        {
            go.transform.parent = AudioManager.Instance.transform;
            go.SetActive(false);
            mGOStack.Push(go);
        }
    }
}