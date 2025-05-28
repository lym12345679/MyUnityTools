using System;
using UnityEngine;

namespace MizukiTool.UIEffect
{
    public class ScaleEffect
    {
        private Transform targetTransform;
        public ScaleEffect()
        {
            duration = 0;
            durationTrick = 0;
            effectMode = ScaleEffectMode.Once;
            isEffectFinish = false;
            isPause = false;
            isFinishImmediately = false;
        }


        #region 缩放效果
        private Vector3 startScale;
        private Vector3 endScale;
        private float duration;
        private float durationTrick;
        private ScaleEffectMode effectMode;
        private int effectPingPongCount;
        private bool isEffectFinish;
        private Func<float, float> effectPercentageHandler;
        private Action<ScaleEffect> effectEndHandler;
        private bool isPause;
        private bool isFinishImmediately;
        public void UpdateScale()
        {
            if (isPause)
            {
                return;
            }
            //Debug.Log("UpdateScale");
            switch (effectMode)
            {
                case ScaleEffectMode.Once:
                    UpdateScaleOnce();
                    break;
                case ScaleEffectMode.PingPong:
                    UpdateScalePingPong();
                    break;
                case ScaleEffectMode.Loop:
                    UpdateScaleLoop();
                    break;
            }
        }
        private void UpdateScaleOnce()
        {
            if (isEffectFinish)
            {
                return;
            }
            if (durationTrick < duration)
            {
                durationTrick += Time.deltaTime;
                float t = durationTrick / duration;
                if (effectPercentageHandler != null)
                {
                    t = effectPercentageHandler(durationTrick / duration);
                }
                targetTransform.localScale = Vector3.Lerp(startScale, endScale, t);
            }
            else
            {
                effectEndHandler?.Invoke(this);
                isEffectFinish = true;
            }
        }
        private void UpdateScalePingPong()
        {
            if (isEffectFinish)
            {
                durationTrick -= Time.deltaTime;
            }
            else
            {
                durationTrick += Time.deltaTime;
            }
            float t = durationTrick / duration;
            if (effectPercentageHandler != null)
            {
                t = effectPercentageHandler(durationTrick / duration);
            }
            targetTransform.localScale = Vector3.Lerp(startScale, endScale, t);

            if (durationTrick >= duration)
            {
                isEffectFinish = true;
            }
            if (durationTrick < 0)
            {
                //Debug.Log("PingPong");
                effectEndHandler?.Invoke(this);
                isEffectFinish = false;
            }
        }
        private void UpdateScaleLoop()
        {
            if (durationTrick > duration)
            {
                durationTrick = 0;
            }
            else
            {
                durationTrick += Time.deltaTime;
            }
            float t = durationTrick / duration;
            if (effectPercentageHandler != null)
            {
                t = effectPercentageHandler(durationTrick / duration);
            }
            targetTransform.localScale = Vector3.Lerp(startScale, endScale, t);
        }
        #endregion
        #region 设置
        /// <summary>
        /// 设置最终缩放比例
        /// </summary>
        public ScaleEffect SetEndScale(Vector3 endScale)
        {
            this.endScale = endScale;
            return this;
        }
        /// <summary>
        /// 设置缩放持续时间
        /// </summary>
        public ScaleEffect SetDuration(float ScaleDuration)
        {
            this.duration = ScaleDuration;
            return this;
        }
        /// <summary>
        /// 设置缩放效果模式
        /// </summary>
        public ScaleEffect SetEffectMode(ScaleEffectMode ScaleEffectMode)
        {
            this.effectMode = ScaleEffectMode;
            return this;
        }
        /// <summary>
        /// 设置缩放百分比处理器
        /// </summary>
        /// <param name="ScaleEffectPercentageHandler">输入为当前线性缩放百分比,输出为修改后的百分比</param>
        public ScaleEffect SetPercentageHandler(Func<float, float> ScaleEffectPercentageHandler)
        {
            this.effectPercentageHandler = ScaleEffectPercentageHandler;
            return this;
        }
        /// <summary>
        /// 设置缩放结束时的处理器，在PingPong模式下每个循环结束都会调用
        /// </summary>
        /// <param name="effectEndHandler">输入为自己的处理器</param>
        public ScaleEffect SetEndHandler(Action<ScaleEffect> effectEndHandler)
        {
            this.effectEndHandler = effectEndHandler;
            return this;
        }
        #endregion
        #region 控制
        /// <summary>
        /// 查看循环次数
        /// </summary>
        public int GetPingPongCount()
        {
            return effectPingPongCount;
        }
        /// <summary>
        /// 查看执行时间
        /// </summary>
        public float GetDurationTrick()
        {
            return durationTrick;
        }
        /// <summary>
        /// 查看是否结束
        /// </summary>
        public bool IsEffectFinish()
        {
            if (isFinishImmediately)
            {
                return true;
            }

            if (effectMode == ScaleEffectMode.Once)
            {
                return isEffectFinish;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 重置位置效果
        /// </summary>
        public void Reset()
        {
            durationTrick = 0;
            isEffectFinish = false;
        }
        /// <summary>
        /// 立刻停止位置效果,并调用结束处理器
        /// </summary>
        public void FinishImmediately()
        {
            StartEndHander();
            isFinishImmediately = true;
        }
        /// <summary>
        /// 立刻停止位置效果
        /// </summary>
        public void Stop()
        {
            isFinishImmediately = true;
        }
        /// <summary>
        /// 暂停位置效果
        /// </summary>
        public void Pause()
        {
            isPause = true;
        }
        /// <summary>
        /// 继续位置效果
        /// </summary>
        public void Continue()
        {
            isPause = false;
        }
        /// <summary>
        /// 立刻调用一次结束处理器
        /// </summary>
        public void StartEndHander()
        {
            this.effectEndHandler?.Invoke(this);
        }
        #endregion
        #region 其他
        public ScaleEffect Start(Transform targetTransform)
        {
            ScaleEffect copyEffect = Copy(this);
            this.targetTransform = targetTransform;
            startScale = targetTransform.localScale;
            return this;
        }
        public ScaleEffect Copy(ScaleEffect ScaleEffect)
        {
            return new ScaleEffect()
            .SetEndScale(ScaleEffect.endScale)
            .SetDuration(ScaleEffect.duration)
            .SetEffectMode(ScaleEffect.effectMode)
            .SetPercentageHandler(ScaleEffect.effectPercentageHandler)
            .SetEndHandler(ScaleEffect.effectEndHandler);
        }
        #endregion
    }
    public enum ScaleEffectMode
    {
        Once,
        PingPong,
        Loop,
    }

}

