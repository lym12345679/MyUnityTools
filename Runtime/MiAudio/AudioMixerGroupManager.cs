using System;
using UnityEngine.Audio;

namespace MizukiTool.MiAudio
{
    //用于管理每组AudioMixer的音量大小
    internal class AudioMixerGroupManager<T> where T : Enum
    {
        //音量范围
        public static float DBMin = -40;
        public static float DBMax = 0;
        public static float DBRange
        {
            get
            {
                return DBMax - DBMin;
            }
        }
        //获取音量百分比
        private static float GetPersentageFromValume(float valume)
        {
            return (valume - DBMin) / DBRange;
        }
        //获取指定AudioMixerGroup的音量大小(返回0~1)
        internal static float GetAudioMixerGroupValume(T audioMixerEnum, AudioMixerGroup entry)
        {
            if (entry != null)
            {
                entry.audioMixer.GetFloat(audioMixerEnum.ToString(), out float value);
                return GetPersentageFromValume(value);
            }
            return 0;
        }

        //设置指定AudioMixerGroup的音量大小(0~1)
        internal static void SetAudioVolume(T audioMixerEnum, float persentage, AudioMixerGroup entry)
        {
            float value = DBMin + DBRange * persentage;
            if (entry != null)
            {
                entry.audioMixer.SetFloat(audioMixerEnum.ToString(), value);
            }
        }
    }
}