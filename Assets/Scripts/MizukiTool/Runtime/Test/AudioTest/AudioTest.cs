using UnityEngine;
using MizukiTool.MiAudio;
namespace MizukiTool.Test.MiAudio
{

    public class AudioTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel1, AudioMixerGroupEnum.BGM, AudioPlayMod.FadeInThenNormal, (context) =>
            {
                TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel2, AudioMixerGroupEnum.BGM, AudioPlayMod.Loop);
            });
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}