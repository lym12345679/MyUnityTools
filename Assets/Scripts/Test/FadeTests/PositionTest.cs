using System.Collections;
using System.Collections.Generic;
using MizukiTool.UIEffect;
using UnityEngine.UI;
using UnityEngine;

public class PositionTest : UIEffectController<Image>
{
    public new Transform transform;
    private PositionEffect positionEffect = new PositionEffect()
        .SetEndPosition(new Vector3(500, 500, 0))
        .SetDuration(1f)
        .SetEffectMode(PositionEffectMode.Loop)
        .SetEndHandler((effect) =>
        {
            Debug.Log("End");
        })
        .SetPercentageHandler((t) =>
        {
            return t * t;
        });
    void Awake()
    {
        transform = GetComponent<Transform>();
    }
    void Start()
    {
        StartPositionEffect(transform, positionEffect);
    }

}
