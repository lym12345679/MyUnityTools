using UnityEngine;
using UnityEngine.UI;
namespace MizukiTool.UIEffect
{
    public class ScaleTest : UIEffectController<Image>
    {
        public new Transform transform;
        private ScaleEffect ScaleEffect = new ScaleEffect()
            .SetEndScale(new Vector3(-1, 1, 0))
            .SetDuration(1f)
            .SetEffectMode(ScaleEffectMode.PingPong)
            .SetEndHandler((effect) =>
            {
                Debug.Log("End");
            })
            .SetPercentageHandler((t) =>
            {
                return t * t;
            });
        private ScaleEffect s;
        void Awake()
        {
            transform = GetComponent<Transform>();
        }
        void Start()
        {
            s = StartScaleEffect(transform, ScaleEffect);
        }
        public override void FixedUpdateNew()
        {
            //Debug.Log(GetScaleEffectCount());
        }
    }

}
