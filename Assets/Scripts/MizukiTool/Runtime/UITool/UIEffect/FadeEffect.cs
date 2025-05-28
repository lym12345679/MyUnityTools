using System;
using UnityEngine;
using UnityEngine.UI;

namespace MizukiTool.UIEffect
{
    public class FadeEffect<T> where T : Graphic
    {

        public void UpdateFade()
        {
            if (isPause)
            {
                return;
            }
            switch (fadeMode)
            {
                case FadeMode.Once:
                    OnceFade();
                    break;
                case FadeMode.Loop:
                    LoopFade();
                    break;
            }
        }
        public FadeEffect()
        {
            fadeTime = 0;
            fadeDelay = 0;
            finalFadeColor = Color.white;
            timeTrick = 0;
            fadeMode = FadeMode.Once;
            FadeTarget = null;
            isFadeFinish = false;
            loopCount = 0;
        }

        #region 渐变属性
        //渐变模式
        private FadeMode fadeMode;
        //渐变时间
        private float fadeTime;
        //渐变延迟
        private float fadeDelay;

        //最终渐变颜色
        private Color finalFadeColor;
        //循环次数
        private int loopCount;
        /// <summary>
        /// 设置渐变时间
        /// </summary>
        public FadeEffect<T> SetFadeTime(float fadeTime)
        {
            this.fadeTime = fadeTime;
            return this;
        }

        /// <summary>
        /// 设置渐变延迟
        /// </summary>
        public FadeEffect<T> SetFadeDelay(float fadeDelay)
        {
            this.fadeDelay = fadeDelay;
            return this;
        }

        /// <summary>
        /// 设置渐变颜色
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public FadeEffect<T> SetFadeColor(Color color)
        {
            this.finalFadeColor = color;
            return this;
        }
        /// <summary>
        /// 设置渐变模式
        /// </summary>
        /// <param name="fadeMode"></param>
        /// <returns></returns>
        public FadeEffect<T> SetFadeMode(FadeMode fadeMode)
        {
            this.fadeMode = fadeMode;
            return this;
        }
        /// <summary>
        /// 设置结束处理
        /// </summary>
        /// <param name="endHander"></param>
        /// <returns></returns>
        public FadeEffect<T> SetEndHander(Action<FadeEffect<T>> endHander)
        {
            this.endHander = endHander;
            return this;
        }
        #endregion

        #region 渐变处理
        public float timeTrick;
        private T FadeTarget;
        public Color OriginalColor;
        private bool isFadeFinish;
        private bool isPause;
        private Action<FadeEffect<T>> endHander;
        private bool isFinishImmediately;
        private Func<float, float> percentageHander;
        public FadeEffect<T> Start(T target)
        {
            FadeEffect<T> copy = Copy(this);
            copy.FadeTarget = target;
            copy.endHander = endHander;
            if (target is Image image)
            {
                copy.OriginalColor = image.color;
            }
            else if (target is Renderer renderer)
            {
                copy.OriginalColor = renderer.material.color;
            }
            return copy;
        }

        public void OnceFade()
        {
            float deltaT = Time.deltaTime;
            timeTrick += deltaT;
            if (timeTrick < fadeDelay || isFadeFinish)
            {
                return;
            }
            float t = (timeTrick - fadeDelay) / fadeTime;
            //Debug.Log(t);

            UpdateColor(t);
            if ((timeTrick - fadeDelay) >= fadeTime)
            {
                isFadeFinish = true;
                endHander?.Invoke(this);
            }

        }
        public void LoopFade()
        {
            if (!isFadeFinish)
            {
                timeTrick += Time.deltaTime;
                if (timeTrick < fadeDelay)
                {
                    return;
                }
            }
            else
            {
                timeTrick -= Time.deltaTime;
            }

            float t = (timeTrick - fadeDelay) / fadeTime;
            if ((timeTrick - fadeDelay) >= fadeTime)
            {
                isFadeFinish = true;
            }
            if (timeTrick <= fadeDelay && isFadeFinish)
            {
                endHander?.Invoke(this);
                loopCount++;
                isFadeFinish = false;
            }
            UpdateColor(t);
        }
        public void UpdateColor(float t)
        {
            //Debug.Log(t);
            if (percentageHander != null)
            {
                t = percentageHander(t);
            }
            if (FadeTarget)
            {
                if (FadeTarget is Image image)
                {
                    image.color = Color.Lerp(OriginalColor, finalFadeColor, t);
                    //Debug.Log(image.color);
                }
            }
            else
            {
                Debug.LogError("FadeTarget is null");
            }
        }
        /// <summary>
        /// 设置缩放百分比处理器
        /// </summary>
        /// <param name="PercentageHandler">输入为当前线性缩放百分比,输出为修改后的百分比</param>
        public FadeEffect<T> SetPersentageHander(Func<float, float> hander)
        {
            this.percentageHander = hander;
            return this;
        }
        #endregion
        #region 其他
        public FadeEffect<T> Copy(FadeEffect<T> fade)
        {
            return new FadeEffect<T>()
            .SetFadeTime(fade.fadeTime)
            .SetFadeDelay(fade.fadeDelay)
            .SetFadeColor(fade.finalFadeColor)
            .SetFadeMode(fade.fadeMode)
            .SetEndHander(fade.endHander);

        }
        public bool IsFadeFinish()
        {
            if (isFinishImmediately)
            {
                return true;
            }
            if (fadeMode == FadeMode.Once)
            {
                return isFadeFinish;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取循环次数
        /// </summary>
        public int GetLoopCount()
        {
            return loopCount;
        }
        /// <summary>
        /// 重置Fade状态
        /// </summary>
        public void Reset()
        {
            timeTrick = 0;
            loopCount = 0;
            isFadeFinish = false;
            UpdateColor(0);
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
        /// 暂时停止,使用Continue继续执行
        /// </summary>
        public void Pause()
        {
            isPause = false;
        }
        /// <summary>
        /// 继续执行,使用Pause暂时停止
        /// </summary>
        public void Continue()
        {
            isPause = true;
        }
        /// <summary>
        /// 改变最终颜色
        /// </summary>
        public void ChangeFinalColor(Color color)
        {
            finalFadeColor = color;
        }
        /// <summary>
        /// 立刻执行一次结束处理
        /// /// </summary>
        public void StartEndHander()
        {
            endHander?.Invoke(this);
        }
        /// <summary>
        /// 改变初始颜色(会忘掉最初的颜色)
        /// </summary>
        public void ChangeOriginalColor(Color color)
        {
            OriginalColor = color;
        }
        #endregion
    }


    public enum FadeMode
    {
        Once,
        Loop,
    }
}
