

using UnityEngine;
using UnityEngine.UI;
namespace MizukiTool.UIEffect
{
    public class FadeTest : UIEffectController<Image>
    {
        private FadeEffect<Image> fade =
        new FadeEffect<Image>().
            SetFadeColor(new Color(1, 1, 1, 0.5f)).
            SetFadeTime(0.1f).
            SetFadeMode(FadeMode.Loop).
            SetFadeDelay(2).SetEndHander((fade) =>
            {
                //Debug.Log("loopCount:" + fade.GetLoopCount());
                fade.ChangeOriginalColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                fade.ChangeFinalColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                //Debug.Log("End");
            });
        private Image image;
        private FadeEffect<Image> fade2;
        void Awake()
        {
            image = GetComponent<Image>();
            if (image == null)
            {
                Debug.LogError("Image is null");
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                fade2.FinishImmediately();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                fade2.Reset();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                fade2.Reset();
                fade2.FinishImmediately();
            }
        }
        private void Start()
        {
            fade2 = StartFade(image, fade);
        }
        public override void FixedUpdateNew()
        {
            //Debug.Log(GetRotationEffectCount());
        }
    }
}

