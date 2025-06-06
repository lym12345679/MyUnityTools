using System;
using MizukiTool.MiAudio;
using UnityEngine;

namespace MizukiTool.Test.MiAudio
{
    public static class TestAudioUtil
    {
        private static bool _isRigister;

        /// <summary>
        ///     音响
        /// </summary>
        public static AudioMixerGroupSO<AudioMixerGroupEnum> audioMixerGroupSO =
            Resources.Load<AudioMixerGroupSO<AudioMixerGroupEnum>>("TestAudioClip/TestAudioMixerGroupSO");

        /// 音效存储参考方式
        public static MizukiTestAudioSO audioSO = Resources.Load<MizukiTestAudioSO>("TestAudioClip/MizukiTestAudioSO");

        private static void SetRigisterAction()
        {
            Action action = () => { RigisterAllAudioClip(); };
            AudioUtil.SetRigisterAction(action);
        }

        private static void RigisterAllAudioClip()
        {
            foreach (var item in audioSO.audioList) RegisterAudioClip(item.audioEnum, item.audioClip);
        }

        private static void RegisterAudioClip<T>(T audioEnum, AudioClip audioClip) where T : Enum
        {
            AudioUtil.RegisterAudioClip(audioEnum, audioClip);
        }

        public static void Play<T>
        (
            T audioEnum,
            AudioMixerGroupEnum audioMixerGroupEnum,
            AudioPlayMod audioPlayMod,
            Action<AudioPlayContext> endEventHander = null,
            Action<AudioPlayContext> fixedUpdateEventHander = null
        ) where T : Enum
        {
            EnsureRigister();
            AudioUtil.Play(audioMixerGroupSO, audioEnum, audioMixerGroupEnum, audioPlayMod, endEventHander,
                fixedUpdateEventHander);
        }

        /// <summary>
        ///     设置音频音量
        /// </summary>
        /// <param name="audioMixerEnum"></param>
        /// <param name="volume"></param>
        public static void SetAudioVolume(AudioMixerGroupEnum audioMixerEnum, float volume)
        {
            EnsureRigister();
            var entry = audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
            AudioUtil.SetAudioVolume(audioMixerEnum, volume, entry);
        }

        /// <summary>
        ///     获取音频音量
        /// </summary>
        /// <param name="audioMixerEnum"></param>
        /// <returns></returns>
        public static float GetAudioValume(AudioMixerGroupEnum audioMixerEnum)
        {
            EnsureRigister();
            var entry = audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
            return AudioUtil.GetAudioVolume(audioMixerEnum, entry);
        }

        private static void EnsureRigister()
        {
            if (!_isRigister)
            {
                _isRigister = true;
                SetRigisterAction();
            }
        }

        #region 循环音频

        /// <summary>
        ///     暂停所有循环音频
        /// </summary>
        public static void PauseAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.PauseAllLoopAudio();
        }

        /// <summary>
        ///     继续所有循环音频
        /// </summary>
        public static void ContinueAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.ContinueAllLoopAudio();
        }

        /// <summary>
        ///     归还所有循环音频
        /// </summary>
        public static void ReturnAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.ReturnAllLoopAudio();
        }

        /// <summary>
        ///     检查枚举是否在循环音频中，如果有则返回对应的AudioClip
        /// </summary>
        /// <param name="audioEnum"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static AudioClip CheckEnumInLoopAudio<T>(T audioEnum) where T : Enum
        {
            EnsureRigister();
            return AudioUtil.CheckEnumInLoopAudio(audioEnum);
        }

        /// <summary>
        ///     根据枚举回收非循环音效
        /// </summary>
        /// <param name="audioEnum"></param>
        /// <typeparam name="T"></typeparam>
        public static void ReturnLoopAudioByEnum<T>(T audioEnum) where T : Enum
        {
            EnsureRigister();
            AudioUtil.ReturnLoopAudioByEnum(audioEnum);
        }

        #endregion

        #region 非循环音频

        /// <summary>
        ///     暂停所有非循环音频
        /// </summary>
        public static void PauseAllNormalAudio()
        {
            EnsureRigister();
            AudioUtil.PauseAllNormalAudio();
        }

        /// <summary>
        ///     继续所有非循环音频
        /// </summary>
        public static void ContinueAllNormalAudio()
        {
            EnsureRigister();
            AudioUtil.ContinueAllNormalAudio();
        }

        /// <summary>
        ///     归还所有非循环音频
        /// </summary>
        public static void ReturnAllNormalAudio()
        {
            EnsureRigister();
            AudioUtil.ReturnAllNormalAudio();
        }

        /// <summary>
        ///     检查枚举是否在非循环音频中，如果有则返回对应的AudioClip
        /// </summary>
        /// <param name="audioEnum"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static AudioClip CheckEnumInNormalAudio<T>(T audioEnum) where T : Enum
        {
            EnsureRigister();
            return AudioUtil.CheckEnumInNormalAudio(audioEnum);
        }

        /// <summary>
        ///     检测是否有指定音效正在播放
        /// </summary>
        /// <param name="audioEnum"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void ReturnNormalAudioByEnum<T>(T audioEnum) where T : Enum
        {
            EnsureRigister();
            AudioUtil.ReturnNormalAudioByEnum(audioEnum);
        }

        #endregion
    }

    public enum AudioMixerGroupEnum
    {
        Master,
        BGM,
        Effect
    }
}