using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MizukiTool.MiAudio;
public class AudioTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel1, AudioMixerGroupEnum.BGM, AudioPlayMod.FadeInThenNormal, (context) =>
        {
            AudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel2, AudioMixerGroupEnum.BGM, AudioPlayMod.Loop);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
