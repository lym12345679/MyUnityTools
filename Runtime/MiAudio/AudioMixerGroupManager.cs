
using UnityEngine;
using UnityEngine.Audio;

namespace MizukiTool.Audio
{
    //用于管理每组AudioMixer的音量大小
    public class AudioMixerGroupManager
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
        internal static float GetAudioMixerGroupValume(AudioMixerGroupEnum audioMixerEnum)
        {
            AudioMixerGroup entry = AudioUtil.audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
            if (entry != null)
            {
                entry.audioMixer.GetFloat(audioMixerEnum.ToString(), out float value);
                return GetPersentageFromValume(value);
            }
            return 0;
        }
        //获取指定AudioMixerGroup
        internal static AudioMixerGroup GetAudioMixerGroup(AudioMixerGroupEnum audioMixerEnum)
        {
            return AudioUtil.audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
        }
        //设置指定AudioMixerGroup的音量大小(0~1)
        internal static void SetAudioVolume(AudioMixerGroupEnum audioMixerEnum, float persentage)
        {
            float value = DBMin + DBRange * persentage;
            AudioMixerGroup entry = AudioUtil.audioMixerGroupSO.GetAudioMixerGroup(audioMixerEnum);
            if (entry != null)
            {
                entry.audioMixer.SetFloat(audioMixerEnum.ToString(), value);
            }
        }
    }
}