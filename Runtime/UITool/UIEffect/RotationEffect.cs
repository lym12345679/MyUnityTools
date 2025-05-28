using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MizukiTool.UIEffect
{
    public class RotationEffect
    {
        private Transform targetTransform;
        public RotationEffect()
        {
            duration = 0;
            durationTrick = 0;
            effectMode = RotationEffectMode.Once;
            isEffectFinish = false;
            isPause = false;
            isFinishImmediately = false;
        }


        #region 缩放效果
        private Vector3 startRotation;
        private Vector3 endRotation;
        private float duration;
        private float durationTrick;
        private RotationEffectMode effectMode;
        private int effectPingPongCount;
        private bool isEffectFinish;
        private Func<float, float> effectPercentageHandler;
        private Action<RotationEffect> effectEndHandler;
        private bool isPause;
        private bool isFinishImmediately;
        public void UpdateRotation()
        {
            if (isPause)
            {
                return;
            }
            //Debug.Log("UpdateRotation");
            switch (effectMode)
            {
                case RotationEffectMode.Once:
                    UpdateRotationOnce();
                    break;
                case RotationEffectMode.PingPong:
                    UpdateRotationPingPong();
                    break;
                case RotationEffectMode.Loop:
                    UpdateRotationLoop();
                    break;
            }
        }
        private void UpdateRotationOnce()
        {
            if (isEffectFinish)
            {
                return;
            }
            if (durationTrick <= duration)
            {
                durationTrick += Time.deltaTime;
                float t = durationTrick / duration;
                if (effectPercentageHandler != null)
                {
                    t = effectPercentageHandler(durationTrick / duration);
                }
                targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));
            }
            else
            {
                effectEndHandler?.Invoke(this);
                isEffectFinish = true;
            }
        }
        private void UpdateRotationPingPong()
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
            targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));

            if (durationTrick >= duration * 0.99)
            {
                isEffectFinish = true;
            }
            if (durationTrick < 0)
            {
                Debug.Log("PingPong");
                effectEndHandler?.Invoke(this);
                isEffectFinish = false;
            }
        }
        private void UpdateRotationLoop()
        {
            if (durationTrick >= duration * .99)
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
            targetTransform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, t));
        }
        #endregion
        #region 设置
        /// <summary>
        /// 设置最终缩放比例
        /// </summary>
        public RotationEffect SetEndRotation(Vector3 endRotation)
        {
            this.endRotation = endRotation;
            return this;
        }
        /// <summary>
        /// 设置缩放持续时间
        /// </summary>
        public RotationEffect SetDuration(float RotationDuration)
        {
            this.duration = RotationDuration;
            return this;
        }
        /// <summary>
        /// 设置缩放效果模式
        /// </summary>
        public RotationEffect SetEffectMode(RotationEffectMode RotationEffectMode)
        {
            this.effectMode = RotationEffectMode;
            return this;
        }
        /// <summary>
        /// 设置缩放百分比处理器
        /// </summary>
        /// <param name="RotationEffectPercentageHandler">输入为当前线性缩放百分比,输出为修改后的百分比</param>
        public RotationEffect SetPercentageHandler(Func<float, float> RotationEffectPercentageHandler)
        {
            this.effectPercentageHandler = RotationEffectPercentageHandler;
            return this;
        }
        /// <summary>
        /// 设置缩放结束时的处理器，在PingPong模式下每个循环结束都会调用
        /// </summary>
        /// <param name="effectEndHandler">输入为自己的处理器</param>
        public RotationEffect SetEndHandler(Action<RotationEffect> effectEndHandler)
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

            if (effectMode == RotationEffectMode.Once)
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
            StartEndHandler();
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
        /// 直接触发结束处理器
        /// </summary>
        public void StartEndHandler()
        {
            effectEndHandler?.Invoke(this);
        }
        #endregion
        #region 其他
        public RotationEffect Start(Transform targetTransform)
        {
            RotationEffect copyEffect = Copy(this);
            this.targetTransform = targetTransform;
            startRotation = targetTransform.localRotation.eulerAngles;
            return this;
        }
        public RotationEffect Copy(RotationEffect RotationEffect)
        {
            return new RotationEffect()
            .SetEndRotation(RotationEffect.endRotation)
            .SetDuration(RotationEffect.duration)
            .SetEffectMode(RotationEffect.effectMode)
            .SetPercentageHandler(RotationEffect.effectPercentageHandler)
            .SetEndHandler(RotationEffect.effectEndHandler);
        }
        #endregion
    }
    public enum RotationEffectMode
    {
        Once,
        PingPong,
        Loop,
    }
}

