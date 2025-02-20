using System;
using System.Collections.Generic;
using UnityEngine;

namespace MizukiTool.MiAudio
{
    public class GeneralAudioSO<T> : ScriptableObject where T : Enum
    {
        public List<AudioClass<T>> audioList = new List<AudioClass<T>>();
        private void OnValidate()
        {
            foreach (var item in audioList)
            {
                item.Name = item.audioEnum.ToString();
            }
        }
        public AudioClip GetAudioClip(T audioEnum)
        {
            foreach (var item in audioList)
            {
                if (item.audioEnum.ToString() == audioEnum.ToString())
                {
                    return item.audioClip;
                }
            }
            return null;
        }
    }
    [Serializable]
    public class AudioClass<T> where T : Enum
    {
        [HideInInspector]
        public string Name;
        public T audioEnum;
        public AudioClip audioClip;
    }

}
