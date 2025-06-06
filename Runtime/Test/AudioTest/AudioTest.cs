using MizukiTool.MiAudio;
using UnityEngine;

namespace MizukiTool.Test.MiAudio
{
    public class AudioTest : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel1, AudioMixerGroupEnum.BGM,
                AudioPlayMod.FadeInThenNormal,
                context =>
                {
                    TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel2, AudioMixerGroupEnum.BGM,
                        AudioPlayMod.Loop);
                });
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnClicked1()
        {
            TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel1, AudioMixerGroupEnum.BGM,
                AudioPlayMod.FadeInThenNormal,
                context =>
                {
                    TestAudioUtil.Play(MizukiTestAudioEnum.BGM_Arknight_Babel2, AudioMixerGroupEnum.BGM,
                        AudioPlayMod.Loop);
                });
        }

        public void OnClicked2()
        {
            TestAudioUtil.ReturnAllLoopAudio();
            TestAudioUtil.ReturnAllNormalAudio();
        }
    }
}