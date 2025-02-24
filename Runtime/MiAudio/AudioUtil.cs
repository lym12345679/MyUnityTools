using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MizukiTool.MiAudio
{
    public static class AudioUtil
    {
        private static Action registerAction;

        /// <summary>
        /// 注册所有的音效(配合RegisterAudioClip使用),在第一次调用音效时立刻触发
        /// </summary>
        private static void RegisterAllAudioClip()
        {
            registerAction?.Invoke();
        }
        public static void SetRigisterAction(Action action)
        {
            registerAction = action;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioEnum">AudioClip对应的枚举</param>
        /// <param name="audioMixerGroupEnum">置入的audioMixerGroup</param>
        /// <param name="audioPlayMod">播放模式</param>
        /// <param name="endEventHander">播放结束时触发的事件</param>
        /// <param name="fixedUpdateEventHander">更新时触发的事件</param>
        /// <returns></returns>
        public static long Play<T1, T2>
        (
            AudioMixerGroupSO<T2> audioMixerGroupSO,
            T1 audioEnum,
            T2 audioMixerGroupEnum,
            AudioPlayMod audioPlayMod,
            Action<AudioPlayContext> endEventHander = null,
            Action<AudioPlayContext> fixedUpdateEventHander = null
        ) where T1 : Enum where T2 : Enum
        {
            EnsureInstance();
            return AudioManager.Instance.Play(audioMixerGroupSO, audioEnum, audioMixerGroupEnum, audioPlayMod, endEventHander, fixedUpdateEventHander);
        }
        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioMixerEnum">音响对应的枚举</param>
        /// <param name="volume">设置值(0~1)</param>
        public static void SetAudioVolume<T>(T audioMixerEnum, float volume, AudioMixerGroup entry) where T : Enum
            => AudioMixerGroupManager<T>.SetAudioVolume(audioMixerEnum, volume, entry);
        /// <summary>
        /// 暂停所有正在循环播放的音效
        /// </summary>
        public static void PauseAllLoopAudio()
        {
            EnsureInstance();
            AudioManager.Instance.PauseAllLoopAudio();
        }
        /// <summary>
        /// 继续所有正在循环播放的音效
        /// </summary> 
        public static void ContinueAllLoopAudio()
        {
            EnsureInstance();
            AudioManager.Instance.ContinueAllLoopAudio();
        }
        /// <summary>
        /// 停止所有正在循环播放的音效
        /// </summary>
        public static void ReturnAllLoopAudio()
        {
            EnsureInstance();
            AudioManager.Instance.ReturnAllLoopAudio();
        }
        /// <summary>
        /// 检测是否有指定音效正在循环播放
        /// </summary>
        /// <param name="audioEnum">该音效所对应的枚举</param>
        /// <returns></returns>
        public static bool CheckEnumInLoopAudio<T>(T audioEnum) where T : Enum
        {
            EnsureInstance();
            return AudioManager.Instance.CheckEnumInLoopAudio(audioEnum);
        }

        /// <summary>
        /// 注册单个音效
        /// </summary>
        public static void RegisterAudioClip<T>(T audioEnum, AudioClip audioClip) where T : Enum
        {
            AudioManager.Instance.RegisterOneAudioClip(audioEnum, audioClip);
        }
        private static void EnsureInstance()
        {
            if (!AudioManager.EnsureInstance())
            {
                RegisterAllAudioClip();
            }
        }
    }


}
