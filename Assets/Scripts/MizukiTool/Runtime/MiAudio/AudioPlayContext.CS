using System;
using UnityEngine;

namespace MizukiTool.MiAudio
{
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
}