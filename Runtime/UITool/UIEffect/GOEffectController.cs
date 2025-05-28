using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.UIEffect
{
    public class GOEffectController<T> : MonoBehaviour where T : Renderer
    {
        private List<FadeEffectGO<T>> fadeList = new List<FadeEffectGO<T>>();
        private List<PositionEffect> positionEffectList = new List<PositionEffect>();
        private List<ScaleEffect> scaleEffectList = new List<ScaleEffect>();
        private List<RotationEffect> rotationEffectList = new List<RotationEffect>();

        private void Update()
        {
            UpdateEffect();
        }

        private void UpdateEffect()
        {
            for (int i = 0; i < fadeList.Count; i++)
            {
                if (fadeList[i].IsFadeFinish())
                {
                    fadeList.RemoveAt(i);
                    i--;
                }
                else
                {
                    fadeList[i].UpdateFade();
                }
            }
            for (int i = 0; i < positionEffectList.Count; i++)
            {
                if (positionEffectList[i].IsEffectFinish())
                {
                    positionEffectList.RemoveAt(i);
                    i--;
                }
                else
                {
                    positionEffectList[i].UpdatePosition();
                }
            }
            for (int i = 0; i < scaleEffectList.Count; i++)
            {
                if (scaleEffectList[i].IsEffectFinish())
                {
                    scaleEffectList.RemoveAt(i);
                    i--;
                }
                else
                {
                    scaleEffectList[i].UpdateScale();
                }
            }
            for (int i = 0; i < rotationEffectList.Count; i++)
            {
                if (rotationEffectList[i].IsEffectFinish())
                {
                    rotationEffectList.RemoveAt(i);
                    i--;
                }
                else
                {
                    rotationEffectList[i].UpdateRotation();
                }
            }
        }

    }
}