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
        /// <summary>
        /// 开始一个渐变效果,开启多个渐变效果可能会冲突
        /// </summary>
        /// <param name="Renderer">Graphic及其子类</param>
        /// <param name="fade">新建的渐变方法</param>
        /// <param name="endHander">每开始一次循环进行执行的函数</param>
        /// <returns>这个渐变方法的实时状态</returns>
        public FadeEffectGO<T> StartFade(T Renderer, FadeEffectGO<T> fade)
        {
            FadeEffectGO<T> copyFade = fade.Start(Renderer);
            fadeList.Add(copyFade);
            return copyFade;
        }
        /// <summary>
        /// 查询当前渐变效果的数量
        /// </summary>
        public int GetFadeCount()
        {
            return fadeList.Count;
        }
        /// <summary>
        /// 开始一个位移效果
        /// </summary>
        /// <param name="targetTransform">Transform</param>
        /// <param name="positionEffect">新建的PositionEffect</param>
        /// <param name="endHander">每结束一次执行的函数</param>
        /// <returns>这个位移方法的实时状态</returns>
        public PositionEffect StartPositionEffect(Transform targetTransform, PositionEffect positionEffect)
        {
            PositionEffect copyPositionEffect = positionEffect.Start(targetTransform);
            positionEffectList.Add(copyPositionEffect);
            return copyPositionEffect;
        }
        /// <summary>
        /// 查询当前位移效果的数量
        /// </summary>
        public int GetPositionEffectCount()
        {
            return positionEffectList.Count;
        }
        /// <summary>
        /// 开始一个缩放效果
        /// </summary>
        /// <param name="targetTransform">Trabsform</param>
        /// <param name="scaleEffect">新建的scaleEffect</param>
        /// <returns>这个缩放方法的实时状态</returns> 
        public ScaleEffect StartScaleEffect(Transform targetTransform, ScaleEffect scaleEffect)
        {
            ScaleEffect copyScaleEffect = scaleEffect.Start(targetTransform);
            scaleEffectList.Add(copyScaleEffect);
            return copyScaleEffect;
        }
        /// <summary>
        /// 查询当前缩放效果的数量
        /// </summary>
        public int GetScaleEffectCount()
        {
            return scaleEffectList.Count;
        }
        /// <summary>
        /// 开始一个旋转效果
        /// </summary>
        /// <param name="targetTransform">Trabsform</param>
        /// <param name="scaleEffect">新建的RotationEffect</param>
        /// <returns>这个旋转方法的实时状态</returns> 
        public RotationEffect StartRotationEffect(Transform targetTransform, RotationEffect rotationEffect)
        {
            RotationEffect copyRotationEffect = rotationEffect.Start(targetTransform);
            rotationEffectList.Add(copyRotationEffect);
            return copyRotationEffect;
        }
        /// <summary>
        /// 查询当前旋转效果的数量
        /// </summary>
        public int GetRotationEffectCount()
        {
            return rotationEffectList.Count;
        }
        private void FixedUpdate()
        {
            UpdateEffect();
            FixedUpdateNew();
        }

        public virtual void FixedUpdateNew()
        {

        }

    }
}