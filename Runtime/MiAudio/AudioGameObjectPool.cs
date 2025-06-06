using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.MiAudio
{
    internal class AudioGameObjectPool
    {
        private readonly Stack<AudioSource> mGOStack = new();
        private GameObject mDisabled;
        private GameObject mEnabled;
        private GameObject mRoot;
        private int mTotal;

        /// <summary>
        ///     初始化
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
        ///     获取对象
        /// </summary>
        /// <returns></returns>
        public AudioSource Get()
        {
            if (mGOStack.Count == 0)
            {
                mTotal++;
                var go = new GameObject("GOPool:" + mTotal);
                var audioSource = go.AddComponent<AudioSource>();
                AudioManager.DontDestroyOnLoad(go);
                return audioSource;
            }

            var target = mGOStack.Pop();
            target.gameObject.SetActive(true);
            return target;
        }

        /// <summary>
        ///     归还对象
        /// </summary>
        /// <param name="go"></param>
        public void Free(AudioSource go)
        {
            go.transform.parent = AudioManager.Instance.transform;
            go.gameObject.SetActive(false);
            mGOStack.Push(go);
        }
    }
}