using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace MizukiTool.MiAudio
{
    [CreateAssetMenu(fileName = "AudioMixerSO", menuName = "MizukiTool/AudioMixerSO")]
    public class AudioMixerGroupSO : ScriptableObject
    {
        public List<AudioMixerClass> audioMixerList = new List<AudioMixerClass>();
        private void OnValidate()
        {
            foreach (var item in audioMixerList)
            {
                item.Name = item.audioMixerEnum.ToString();
            }
        }
        public AudioMixerGroup GetAudioMixerGroup(AudioMixerGroupEnum audioMixerEnum)
        {
            foreach (var item in audioMixerList)
            {
                if (item.audioMixerEnum == audioMixerEnum)
                {
                    return item.audioMixerGroup;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class AudioMixerClass
    {
        [HideInInspector]
        public string Name;
        public AudioMixerGroupEnum audioMixerEnum;
        public AudioMixerGroup audioMixerGroup;
    }



}
