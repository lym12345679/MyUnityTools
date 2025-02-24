using System;
using MizukiTool.MiAudio;
using UnityEngine;
using UnityEngine.Audio;

namespace MizukiTool.Test.MiAudio
{
    public static class TestAudioUtil
    {
        private static bool isRigister = false;
        /// <summary>
        /// 音响
        /// </summary>
        public static AudioMixerGroupSO<AudioMixerGroupEnum> audioMixerGroupSO = Resources.Load<AudioMixerGroupSO<AudioMixerGroupEnum>>("TestAudioClip/TestAudioMixerGroupSO");
        /// 音效存储参考方式
        public static MizukiTestAudioSO audioSO = Resources.Load<MizukiTestAudioSO>("TestAudioClip/MizukiTestAudioSO");
        private static void SetRigisterAction()
        {
            Action action = () => { RigisterAllAudioClip(); };
            AudioUtil.SetRigisterAction(action);
        }
        private static void RigisterAllAudioClip()
        {
            foreach (var item in audioSO.audioList)
            {
                RegisterAudioClip(item.audioEnum, item.audioClip);
            }
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
            AudioUtil.Play(audioMixerGroupSO, audioEnum, audioMixerGroupEnum, audioPlayMod, endEventHander, fixedUpdateEventHander);
        }
        public static void SetAudioVolume(AudioMixerGroupEnum audioMixerEnum, float volume)
        {
            EnsureRigister();
            AudioMixerGroup entry = audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
            AudioUtil.SetAudioVolume(audioMixerEnum, volume, entry);
        }
        public static void PauseAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.PauseAllLoopAudio();
        }
        public static void ContinueAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.ContinueAllLoopAudio();
        }
        public static void ReturnAllLoopAudio()
        {
            EnsureRigister();
            AudioUtil.ReturnAllLoopAudio();
        }

        public static bool CheckEnumInLoopAudio<T>(T audioEnum) where T : Enum
        {
            EnsureRigister();
            return AudioUtil.CheckEnumInLoopAudio(audioEnum);
        }

        private static void EnsureRigister()
        {
            if (!isRigister)
            {
                isRigister = true;
                SetRigisterAction();
            }
        }

    }
    public enum AudioMixerGroupEnum
    {
        Master,
        BGM,
        Effect
    }
}