using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace MizukiTool.MiAudio
{
    /// <summary>
    /// 音频混合器组容器
    /// </summary> 
    public class AudioMixerGroupSO<T> : ScriptableObject where T : Enum
    {
        public List<AudioMixerClass<T>> audioMixerList = new List<AudioMixerClass<T>>();
        private void OnValidate()
        {
            foreach (var item in audioMixerList)
            {
                item.Name = item.audioMixerEnum.ToString();
            }
        }
        /// <summary>
        /// 获取指定AudioMixerGroup
        /// </summary>
        /// <param name="audioMixerEnum"></param>
        /// <returns></returns>
        public AudioMixerGroup GetAudioMixerGroup(T audioMixerEnum)
        {
            foreach (var item in audioMixerList)
            {
                if (item.audioMixerEnum.ToString() == audioMixerEnum.ToString())
                {
                    return item.audioMixerGroup;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class AudioMixerClass<T> where T : Enum
    {
        [HideInInspector]
        public string Name;
        public T audioMixerEnum;
        public AudioMixerGroup audioMixerGroup;
    }



}
