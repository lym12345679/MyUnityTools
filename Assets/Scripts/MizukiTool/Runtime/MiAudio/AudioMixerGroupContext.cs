/*using System;
using UnityEngine;
using UnityEngine.Audio;

namespace MizukiTool.MiAudio
{
    public class AudioMixerGroupContext<T> where T : Enum
    {
        /// <summary>
        /// 混音器
        /// </summary>
        public AudioMixer mAudioMixer;
        /// <summary>
        /// U3D混音组
        /// </summary>
        public AudioMixerGroup mAudioMixerGroup;
        /// <summary>
        /// 音量参数名字
        /// </summary>
        public string mVolumeName;
        /// <summary>
        /// 当前音量
        /// </summary>
        public float mVolume = -1;
        public string Name
        {
            get
            {
                return mAudioMixerGroup.name;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mixer">混音器</param>
        /// <param name="name">混音组名字</param>
        /// <returns></returns>
        public bool Init(AudioMixer mixer, string name)
        {
            mAudioMixer = mixer;
            AudioMixerGroup[] groups = mAudioMixer.FindMatchingGroups(name);
            if (groups == null || groups.Length == 0)
            {
                // Erro log
                return false;
            }
            mAudioMixerGroup = groups[0];
            mVolumeName = "Vol_" + name;
            return true;
        }
        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="v"></param>
        public void SetVolume(float v)
        {
            if (mAudioMixerGroup == null || mVolume == v)
                return;
            mVolume = Mathf.Clamp01(v);
            float dbVolume = mVolume > 0 ? AudioMixerGroupManager<T>.DBMin + AudioMixerGroupManager<T>.DBRange * v : -80;
            mAudioMixer.SetFloat(mVolumeName, dbVolume);
        }
        /// <summary>
        /// 获取音量
        /// </summary>
        /// <returns></returns>
        public float GetVolume()
        {
            if (mAudioMixerGroup == null)
                return -1;
            if (mVolume < 0)
            {
                float dbVolume = 0;
                mAudioMixer.GetFloat(mVolumeName, out dbVolume);
                mVolume = (AudioMixerGroupManager<T>.DBMax - dbVolume) / AudioMixerGroupManager<T>.DBRange;
            }
            return mVolume;
        }
    }
}*/