using System;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.MiAudio
{
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// 单体实例
        /// </summary>
        public static AudioManager Instance;
        /// <summary>
        /// 音效播放对象池，用于在指定位置播放音效
        /// </summary>
        private GameObjectPool mAudioSourceObjectPool = new GameObjectPool();
        /// <summary>
        /// 下个音效ID
        /// </summary>
        private long mNextAudioID = 1;
        /// <summary>
        /// 音效字典，存放所有正在播放的音效
        /// </summary>
        private Dictionary<long, AudioPlayContext> mAudioContextDic = new Dictionary<long, AudioPlayContext>();
        /// <summary>
        /// 淡入的音效
        /// </summary>
        private List<AudioPlayContext> mAudioContextInFadingIn = new List<AudioPlayContext>();
        /// <summary>
        /// 淡出的音效
        /// </summary>
        private List<AudioPlayContext> mAudioContextInFadingOut = new List<AudioPlayContext>();
        /// <summary>
        /// 循环播放的音效
        /// </summary>
        private List<AudioPlayContext> mAudioEntryInLoop = new List<AudioPlayContext>();
        /// <summary>
        /// 非循环，等待结束的音效
        /// </summary>
        private List<AudioPlayContext> mAudioContextWaitFinish = new List<AudioPlayContext>();
        /// <summary>
        /// 未使用的AudioPlayEntry
        /// </summary>
        private List<AudioPlayContext> UnusedAudioPlayEntry = new List<AudioPlayContext>();
        /// <summary>
        /// 音效字典,在启动时注册所有的音效
        /// </summary>
        private Dictionary<string, AudioClip> mAudioClipDic = new Dictionary<string, AudioClip>();
        private EnumIdentifier mEnumIdentifier = new EnumIdentifier();
        void Start()
        {
            OnStart();
        }
        void Update()
        {
            OnFixedUpdate();
        }
        /// <summary>
        /// 启动
        /// </summary>
        private void OnStart()
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        /// <summary>
        /// 注册音效
        /// </summary> 
        internal void RegisterOneAudioClip<T>(T audioEnum, AudioClip audioClip) where T : Enum
        {
            mEnumIdentifier.SetEnum(audioEnum);
            mAudioClipDic.Add(mEnumIdentifier.GetID(), audioClip);
        }
        /// <summary>
        /// 创建新的音效容器
        /// </summary> 
        private void CreatNewAudioPlayEntry()
        {
            AudioPlayContext audioPlayEntry = new AudioPlayContext(mNextAudioID);
            UnusedAudioPlayEntry.Add(audioPlayEntry);
            mNextAudioID++;
        }
        /// <summary>
        /// 获取音效容器 
        /// </summary>
        /// <returns></returns>
        public AudioPlayContext GetAudioPlayContext()
        {
            if (UnusedAudioPlayEntry.Count == 0)
            {
                CreatNewAudioPlayEntry();
            }
            AudioPlayContext audioPlayEntry = UnusedAudioPlayEntry[0];
            UnusedAudioPlayEntry.RemoveAt(0);
            audioPlayEntry.Init();
            GameObject go = mAudioSourceObjectPool.Get();
            audioPlayEntry.SetTargetGO(go.transform);
            return audioPlayEntry;
        }
        //回收音效
        public bool RetrunAudioPlayEntry(AudioPlayContext audioPlayEntry)
        {
            if (audioPlayEntry == null)
            {
                return false;
            }
            if (audioPlayEntry.TargetAudioSource.isPlaying)
            {
                audioPlayEntry.TargetAudioSource.Stop();
            }
            UnusedAudioPlayEntry.Add(audioPlayEntry);
            mAudioSourceObjectPool.Free(audioPlayEntry.SelfTransform.gameObject);
            audioPlayEntry.SetTargetGO(null);
            return true;
        }
        //根据播放模式播放音乐
        public void StartAudioPlayEntry(AudioPlayMod audioPlayMod, AudioPlayContext audioPlayEntry)
        {
            switch (audioPlayMod)
            {
                case AudioPlayMod.Normal:
                    {
                        mAudioContextDic.Add(audioPlayEntry.ID, audioPlayEntry);
                        mAudioContextWaitFinish.Add(audioPlayEntry);
                    }
                    break;
                case AudioPlayMod.Loop:
                    {
                        mAudioContextDic.Add(audioPlayEntry.ID, audioPlayEntry);
                        audioPlayEntry.TargetAudioSource.loop = true;
                        mAudioEntryInLoop.Add(audioPlayEntry);
                    }
                    break;
                case AudioPlayMod.FadeInThenNormal:
                    {
                        mAudioContextDic.Add(audioPlayEntry.ID, audioPlayEntry);
                        mAudioContextInFadingIn.Add(audioPlayEntry);
                        audioPlayEntry.TargetAudioSource.volume = 0;
                        //Debug.Log("FadeInThenNormal");
                    }
                    break;
                case AudioPlayMod.FadeInThenLoop:
                    {
                        mAudioContextDic.Add(audioPlayEntry.ID, audioPlayEntry);
                        mAudioContextInFadingIn.Add(audioPlayEntry);
                    }
                    break;
            }
            audioPlayEntry.Play();
        }
        //持续查看音效是否结束
        private void OnFixedUpdate()
        {

            //处理淡入淡出音效
            for (int i = mAudioContextInFadingIn.Count - 1; i >= 0; i--)
            {
                var audioEntry = mAudioContextInFadingIn[i];
                if (audioEntry.TargetAudioSource.isPlaying)
                {
                    float volume = audioEntry.TargetAudioSource.volume + Time.deltaTime;
                    if (volume > 1)
                    {
                        switch (audioEntry.TheAudioPlayMod)
                        {
                            case AudioPlayMod.FadeInThenNormal:
                                {
                                    mAudioContextInFadingIn.Remove(audioEntry);
                                    mAudioContextWaitFinish.Add(audioEntry);
                                }
                                break;
                            case AudioPlayMod.FadeInThenLoop:
                                {
                                    mAudioContextInFadingIn.Remove(audioEntry);
                                    mAudioEntryInLoop.Add(audioEntry);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        audioEntry.TargetAudioSource.volume = Mathf.Min(1, audioEntry.TargetAudioSource.volume + Time.deltaTime);
                    }
                }
                else
                {
                    mAudioContextDic.Remove(audioEntry.ID);
                    mAudioContextInFadingIn.Remove(audioEntry);
                    RetrunAudioPlayEntry(audioEntry);

                }
            }


            for (int i = mAudioContextInFadingOut.Count - 1; i >= 0; i--)
            {
                var audioEntry = mAudioContextInFadingOut[i];
                if (audioEntry.TargetAudioSource.isPlaying)
                {
                    audioEntry.TargetAudioSource.volume = Mathf.Max(0, audioEntry.TargetAudioSource.volume - Time.deltaTime);
                }
                else
                {
                    mAudioContextDic.Remove(audioEntry.ID);
                    mAudioContextInFadingIn.Remove(audioEntry);
                    RetrunAudioPlayEntry(audioEntry);
                }
            }
            //处理非循环音效
            for (int i = mAudioContextWaitFinish.Count - 1; i >= 0; i--)
            {
                var audioContext = mAudioContextWaitFinish[i];
                if (!audioContext.IsPlaying())
                {
                    audioContext.OnAudioEnd();
                    mAudioContextDic.Remove(audioContext.ID);
                    mAudioContextWaitFinish.Remove(audioContext);
                    RetrunAudioPlayEntry(audioContext);
                }
            }
            //处理循环音效
            foreach (var audioContext in mAudioContextDic)
            {
                audioContext.Value.OnUpdate();
            }
        }
        //播放音效，返回使用的容器ID
        public long Play<T>(T audioEnum, AudioMixerGroupEnum audioMixerGroupEnum, AudioPlayMod audioPlayMod, Action<AudioPlayContext> endEventHander = null, Action<AudioPlayContext> updateEventHander = null) where T : Enum
        {
            EnsureInstance();
            AudioPlayContext audioPlayContext = GetAudioPlayContext();
            mEnumIdentifier.SetEnum(audioEnum);

            audioPlayContext.TargetAudioSource.clip = mAudioClipDic[mEnumIdentifier.GetID()];
            audioPlayContext.TargetAudioSource.outputAudioMixerGroup = AudioMixerGroupManager.GetAudioMixerGroup(audioMixerGroupEnum);
            if (endEventHander != null)
            {
                audioPlayContext.SetEndHander(endEventHander);
            }
            if (updateEventHander != null)
            {
                audioPlayContext.SetUpdateHander(updateEventHander);
            }
            audioPlayContext.TheAudioPlayMod = audioPlayMod;
            StartAudioPlayEntry(audioPlayMod, audioPlayContext);
            return audioPlayContext.ID;
        }
        //单例
        public static bool EnsureInstance()
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                Instance = go.AddComponent<AudioManager>();
                return false;
            }
            return true;
        }
        //暂停所有循环音效
        public void PauseAllLoopAudio()
        {
            for (int i = mAudioEntryInLoop.Count - 1; i >= 0; i--)
            {
                var audioEntry = mAudioEntryInLoop[i];
                audioEntry.Pause();
            }
        }
        //继续所有循环音效
        public void ContinueAllLoopAudio()
        {
            for (int i = mAudioEntryInLoop.Count - 1; i >= 0; i--)
            {
                var audioEntry = mAudioEntryInLoop[i];
                audioEntry.UnPause();
            }
        }
        //回收所有循环音效
        public void ReturnAllLoopAudio()
        {
            for (int i = mAudioEntryInLoop.Count - 1; i >= 0; i--)
            {
                var audioEntry = mAudioEntryInLoop[i];
                mAudioContextDic.Remove(audioEntry.ID);
                mAudioEntryInLoop.Remove(audioEntry);
                RetrunAudioPlayEntry(audioEntry);
            }
        }
        /// <summary>
        /// 判断是否在循环播放
        /// </summary>
        /// <param name="audioEnum"></param>
        /// <returns></returns>
        public bool CheckEnumInLoopAudio(MizukiTestAudioEnum audioEnum)
        {
            mEnumIdentifier.SetEnum(audioEnum);
            string audioEnumID = mEnumIdentifier.GetID();
            foreach (var audioEntry in mAudioEntryInLoop)
            {
                if (audioEntry.TargetAudioSource.clip == mAudioClipDic[audioEnumID])
                {
                    return true;
                }
            }
            return false;
        }
    }
    public class AudioPlayContext
    {
        private bool isPlaying = false;
        public readonly long ID;
        public AudioPlayMod TheAudioPlayMod;
        private AudioSource targetAudioSource;
        public AudioSource TargetAudioSource
        {
            get
            {
                if (targetAudioSource != null)
                {
                    return targetAudioSource;
                }
                if (SelfTransform != null)
                {
                    if (!SelfTransform.TryGetComponent(out targetAudioSource))
                    {
                        targetAudioSource = SelfTransform.gameObject.AddComponent<AudioSource>();
                    }
                    return targetAudioSource;
                }
                return null;
            }
        }
        private Transform selfTransform;
        //绑定的物体
        public Transform SelfTransform
        {
            get
            {
                return selfTransform;
            }
            private set
            {
                selfTransform = value;
            }
        }
        //播放结束时的处理
        public Action<AudioPlayContext> endHander;
        //播放刷新时的处理
        public Action<AudioPlayContext> fixedUpdateHander;
        public AudioPlayContext(long ID)
        {
            this.ID = ID;
            SelfTransform = null;
            endHander = null;
            fixedUpdateHander = null;
            TheAudioPlayMod = AudioPlayMod.Normal;
        }
        public void Init()
        {
            SelfTransform = null;
            endHander = null;
            fixedUpdateHander = null;
            TheAudioPlayMod = AudioPlayMod.Normal;
            if (targetAudioSource != null)
            {
                targetAudioSource.volume = 1;
                targetAudioSource.pitch = 1;
            }

        }
        public AudioPlayContext SetPosition(Vector3 position)
        {
            this.SelfTransform.position = position;
            return this;
        }
        public AudioPlayContext SetTargetGO(Transform targetGO)
        {
            this.SelfTransform = targetGO;
            return this;
        }
        public AudioPlayContext SetEndHander(Action<AudioPlayContext> endHander)
        {
            this.endHander = endHander;
            return this;
        }
        public AudioPlayContext SetUpdateHander(Action<AudioPlayContext> updateHander)
        {
            this.fixedUpdateHander = updateHander;
            return this;
        }
        public void Play()
        {
            if (SelfTransform != null)
            {
                TargetAudioSource.transform.position = SelfTransform.position;
            }
            isPlaying = true;
            TargetAudioSource.Play();
        }
        public void Stop()
        {
            TargetAudioSource.Stop();
        }
        public void Pause()
        {
            TargetAudioSource.Pause();
        }
        public void UnPause()
        {
            TargetAudioSource.UnPause();
        }
        /// <summary>
        /// 设置是否循环播放
        /// </summary>
        /// <param name="loop"></param>
        /// <returns></returns>
        public AudioPlayContext SetLoop(bool loop)
        {
            if (loop)
            {
                TheAudioPlayMod = AudioPlayMod.Loop;
            }
            TargetAudioSource.loop = loop;
            return this;
        }
        /// <summary>
        /// 单独设置音量大小
        /// </summary>
        public AudioPlayContext SetVolume(float volume)
        {
            TargetAudioSource.volume = volume;
            return this;
        }
        /// <summary>
        /// 设置音调
        /// </summary>
        public void SetPitch(float pitch)
        {
            TargetAudioSource.pitch = pitch;
        }
        public void OnAudioEnd()
        {
            if (endHander != null)
            {
                endHander(this);
            }
        }
        public void OnUpdate()
        {
            if (fixedUpdateHander != null)
            {
                fixedUpdateHander(this);
            }
            if (!TargetAudioSource.isPlaying)
            {
                isPlaying = false;
            }
        }
        public bool IsPlaying()
        {
            return isPlaying;
        }

    }
    public enum AudioPlayMod
    {
        /// <summary>
        /// 普通播放
        /// </summary>
        Normal,
        /// <summary>
        /// 循环播放
        /// </summary>
        Loop,
        /// <summary>
        /// 淡入后不循环
        /// </summary>
        FadeInThenNormal,
        /// <summary>
        /// 淡入后循环
        /// </summary>
        FadeInThenLoop,
    }
}
