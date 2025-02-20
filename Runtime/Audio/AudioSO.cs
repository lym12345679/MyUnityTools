using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.Audio
{
    [CreateAssetMenu(fileName = "AudioSO", menuName = "MizukiTool/AudioSO")]
    public class AudioSO : ScriptableObject
    {
        public List<AudioClass> audioList = new List<AudioClass>();
        private void OnValidate()
        {
            foreach (var item in audioList)
            {
                item.Name = item.audioEnum.ToString();
            }
        }
        public AudioClip GetAudioClip(AudioEnum audioEnum)
        {
            foreach (var item in audioList)
            {
                if (item.audioEnum == audioEnum)
                {
                    return item.audioClip;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class AudioClass
    {
        [HideInInspector]
        public string Name;
        public AudioEnum audioEnum;
        public AudioClip audioClip;
    }

}
