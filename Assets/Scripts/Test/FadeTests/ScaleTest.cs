using System.Collections;
using System.Collections.Generic;
using MizukiTool.UIEffect;
using UnityEngine;
using UnityEngine.UI;

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
        base.FixedUpdateNew();
        //Debug.Log(GetScaleEffectCount());
    }
}
