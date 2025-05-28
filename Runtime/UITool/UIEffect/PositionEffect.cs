using System;
using UnityEngine;
namespace MizukiTool.UIEffect
{
    public class PositionEffect
    {
        private Transform targetTransform;
        public PositionEffect()
        {
            duration = 0;
            durationTrick = 0;
            effectMode = PositionEffectMode.Once;
            isEffectFinish = false;
            isPause = false;
            isFinishImmediately = false;
        }


        #region 位置效果
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float duration;
        private float durationTrick;
        private PositionEffectMode effectMode;
        private int effectPingPongCount;
        private bool isEffectFinish;
        private Func<float, float> effectPercentageHandler;
        private Action<PositionEffect> effectEndHandler;
        private bool isPause;
        private bool isFinishImmediately;
        public void UpdatePosition()
        {
            if (isPause)
            {
                return;
            }
            //Debug.Log("UpdatePosition");
            switch (effectMode)
            {
                case PositionEffectMode.Once:
                    UpdatePositionOnce();
                    break;
                case PositionEffectMode.PingPong:
                    UpdatePositionPingPong();
                    break;
                case PositionEffectMode.Loop:
                    UpdatePositionLoop();
                    break;
            }
        }
        private void UpdatePositionOnce()
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
                targetTransform.position = Vector3.Lerp(startPosition, endPosition, t);
            }
            else
            {
                effectEndHandler?.Invoke(this);
                isEffectFinish = true;
            }
        }
        private void UpdatePositionPingPong()
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
            targetTransform.position = Vector3.Lerp(startPosition, endPosition, t);

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

        private void UpdatePositionLoop()
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
            targetTransform.position = Vector3.Lerp(startPosition, endPosition, t);

        }
        #endregion
        #region 设置
        /// <summary>
        /// 设置最终位置
        /// </summary>
        public PositionEffect SetEndPosition(Vector3 endPosition)
        {
            this.endPosition = endPosition;
            return this;
        }
        /// <summary>
        /// 设置位移持续时间
        /// </summary>
        public PositionEffect SetDuration(float positionDuration)
        {
            this.duration = positionDuration;
            return this;
        }
        /// <summary>
        /// 设置位移效果模式
        /// </summary>
        public PositionEffect SetEffectMode(PositionEffectMode positionEffectMode)
        {
            this.effectMode = positionEffectMode;
            return this;
        }
        /// <summary>
        /// 设置位移百分比处理器
        /// </summary>
        /// <param name="positionEffectPercentageHandler">输入为当前线性位移百分比,输出为修改后的百分比</param>
        public PositionEffect SetPercentageHandler(Func<float, float> positionEffectPercentageHandler)
        {
            this.effectPercentageHandler = positionEffectPercentageHandler;
            return this;
        }
        /// <summary>
        /// 设置位移结束时的处理器，在PingPong模式下每个循环结束都会调用
        /// </summary>
        /// <param name="effectEndHandler">输入为自己的处理器</param>
        public PositionEffect SetEndHandler(Action<PositionEffect> effectEndHandler)
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

            if (effectMode == PositionEffectMode.Once)
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
        /// <sum
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
        /// 立刻触发结束处理器
        /// </summary> 
        public void StartEndHander()
        {
            effectEndHandler?.Invoke(this);
        }
        #endregion
        #region 其他
        public PositionEffect Start(Transform targetTransform)
        {
            PositionEffect copyEffect = Copy(this);
            this.targetTransform = targetTransform;
            startPosition = targetTransform.position;
            return this;
        }
        public PositionEffect Copy(PositionEffect positionEffect)
        {
            return new PositionEffect()
            .SetEndPosition(positionEffect.endPosition)
            .SetDuration(positionEffect.duration)
            .SetEffectMode(positionEffect.effectMode)
            .SetPercentageHandler(positionEffect.effectPercentageHandler)
            .SetEndHandler(positionEffect.effectEndHandler);
        }
        #endregion
    }
    public enum PositionEffectMode
    {
        Once,
        PingPong,
        Loop,
    }
}

