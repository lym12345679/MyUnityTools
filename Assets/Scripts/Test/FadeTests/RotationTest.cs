using System.Collections;
using System.Collections.Generic;
using MizukiTool.UIEffect;
using UnityEngine;
using UnityEngine.UI;

public class RotationTest : UIEffectController<Image>
{
    public new Transform transform;
    private RotationEffect RotationEffect = new RotationEffect()
        .SetEndRotation(new Vector3(0, 0, 360))
        .SetDuration(1f)
        .SetEffectMode(RotationEffectMode.Loop)
        .SetEndHandler((effect) =>
        {
            Debug.Log("End");
        });
    private RotationEffect s;
    void Awake()
    {
        transform = GetComponent<Transform>();
    }
    void Start()
    {
        s = StartRotationEffect(transform, RotationEffect);
    }
    public override void FixedUpdateNew()
    {
        base.FixedUpdateNew();
        //Debug.Log(GetRotationEffectCount());
    }
}
