/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/02/04 23:14:51
** desc:  ʱ�����
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class TimerMgr : MonoSingleton<TimerMgr>
    {
        #region Time Sync

        /// <summary>
        /// ������ͬ��������ʱ��;
        /// </summary>
        private DateTime serverTime;

        /// <summary>
        /// �ͻ��˿�����Ϸʱ������ʱ��,��λ��;
        /// </summary>
        private float clientStartTime;

        /// <summary>
        /// ��ǰ������ʱ��;
        /// </summary>
        public DateTime CurentSeverTime
        {
            get
            {
                return serverTime.AddSeconds(Time.realtimeSinceStartup - clientStartTime);
            }
        }
        /// <summary>
        /// ͬ����������ʱ�䵽�ͻ���;
        /// </summary>
        /// <param name="time">������ʱ��</param>
        public void SyncServerTime(DateTime time)
        {
            serverTime = time;
            clientStartTime = Time.realtimeSinceStartup;
            LogUtil.LogUtility.PrintWarning(string.Format("[Sync Server Time] ServerTime = {0}", serverTime));
        }

        #endregion

        #region Timer Event

        /// <summary>
        /// ��ʱ���¼�;
        /// </summary>
        public class TimerEvent
        {
            /// <summary>
            /// �״�ִ����ʱ;
            /// </summary>
            public float DelayTime;

            /// <summary>
            /// ִ�д���;
            /// </summary>
            public int RepeatTimes;

            /// <summary>
            /// �Ѿ�ִ�еĴ���;
            /// </summary>
            public int ExeTimes;

            /// <summary>
            /// ִ�м��;
            /// </summary>
            public float IntervalTime;

            /// <summary>
            /// �޲λص�����;
            /// </summary>
            public Action CallBackFunc;

            /// <summary>
            /// ����;
            /// </summary>
            public object Param;

            /// <summary>
            /// ���������Ļص�����;
            /// </summary>
            public Action<object> CallBackFuncWithParam;

            /// <summary>
            /// ��ʱ���¼�������Ļص�����;
            /// </summary>
            public Action OnFinish;

            /// <summary>
            /// �¼���ʼʱ��;
            /// </summary>
            public float StartTime;

            /// <summary>
            /// ��һ������ʱ��;
            /// </summary>
            public float NextRunTime;

            /// <summary>
            /// �¼��Ƿ��Ѿ�����;
            /// </summary>
            public bool IsFinish;

            /// <summary>
            /// �Ƿ���ͣ;
            /// </summary>
            public bool IsPause;

            /// <summary>
            /// ��ǰʱ��;
            /// </summary>
            public float CurTime
            {
                get { return Time.realtimeSinceStartup; }
            }

            /// <summary>
            /// ʱ���;
            /// </summary>
            public float DeltaTime
            {
                get { return Time.unscaledDeltaTime; }
            }

            /// <summary>
            /// ʱ���ʼ��;
            /// </summary>
            public void InitTimerEvent()
            {
                StartTime = CurTime;
                NextRunTime = StartTime + DelayTime;
            }

            /// <summary>
            /// ����ʱ�Ƴ������¼�;
            /// </summary>
            public void RemoveCallBackFunc()
            {
                CallBackFunc = null;
                CallBackFuncWithParam = null;
                OnFinish = null;
            }

            /// <summary>
            /// ִ�лص�;
            /// </summary>
            public void DoCallBack()
            {
                if (CallBackFunc != null)
                {
                    CallBackFunc();
                }
                if (CallBackFuncWithParam != null)
                {
                    CallBackFuncWithParam(Param);
                }
            }

            /// <summary>
            /// �Ƴ��¼��жϺ���;
            /// </summary>
            /// <returns></returns>
            public bool IsCanRemoveEvent()
            {
                if (IsFinish)
                {
                    return true;
                }
                if (CountDown())
                {
                    DoCallBack();
                    if (RepeatTimes >= 0 && ExeTimes >= RepeatTimes)
                    {
                        return true;
                    }
                    ExeTimes++;
                }
                return false;
            }

            /// <summary>
            /// ��ʱ����;
            /// </summary>
            /// <returns></returns>
            protected bool CountDown()
            {
                if (IsPause)
                {
                    NextRunTime += DeltaTime;
                }
                else if (NextRunTime <= CurTime)
                {
                    NextRunTime += IntervalTime;
                    return true;
                }
                return false;
            }
        }

        #endregion


        #region Timer Event Register

        private List<TimerEvent> EventLists = new List<TimerEvent>();

        /// <summary>
        /// ע���ʱ�¼�;
        /// </summary>
        /// <param name="delayTime">��һ��ִ����ʱ(s)</param>
        /// <param name="repeatTimes">�ظ�ִ�д���(s)</param>
        /// <param name="intervalTime">����ִ�м��ʱ��(s)</param>
        /// <param name="callBackFunc">�޲λص�����</param>
        /// <param name="callBackFuncWithParam">�ص�����</param>
        /// <param name="param">����</param>
        /// <param name="onFinish">�¼������ص�����</param>
        /// <returns></returns>
        public TimerHandler RegisterTimerEvent(float delayTime, int repeatTimes, float intervalTime, Action callBackFunc,
            Action<object> callBackFuncWithParam, object param, Action onFinish)
        {
            TimerEvent timerEvent = new TimerEvent
            {
                DelayTime = delayTime,
                RepeatTimes = repeatTimes,
                IntervalTime = intervalTime,
                CallBackFunc = callBackFunc,
                CallBackFuncWithParam = callBackFuncWithParam,
                Param = param,
                OnFinish = onFinish,
                IsFinish = false,
                IsPause = false
            };
            timerEvent.InitTimerEvent();
            EventLists.Add(timerEvent);
            TimerHandler handler = new TimerHandler(timerEvent);
            return handler;
        }

        /// <summary>
        /// ע���ʱ�¼�;
        /// </summary>
        /// <param name="delayTime">��һ��ִ����ʱ(s)</param>
        /// <param name="callBackFunc">�ص�����</param>
        /// <param name="onFinish">�����ص�����</param>
        /// <returns></returns>
        public TimerHandler RegisterTimerEvent(float delayTime, Action callBackFunc, Action onFinish)
        {
            return RegisterTimerEvent(delayTime, 0, 1, callBackFunc, null, null, onFinish);
        }

        #endregion

        #region Timer Handler

        /// <summary>
        /// ��ʱ�¼�������;
        /// </summary>
        public class TimerHandler
        {
            /// <summary>
            /// ��ʱ�¼�;
            /// </summary>
            private TimerEvent mTimer;

            public TimerHandler(TimerEvent timer)
            {
                mTimer = timer;
            }
            /// <summary>
            /// ��ͣ;
            /// </summary>
            public void Pause()
            {
                if (mTimer != null) mTimer.IsPause = true;
            }
            /// <summary>
            /// ����;
            /// </summary>
            public void Replay()
            {
                if (mTimer != null) mTimer.IsPause = false;
            }
            /// <summary>
            /// �Ƴ�ʱ��;
            /// </summary>
            public void RemoveTimer()
            {
                mTimer.IsFinish = true;
                mTimer.RemoveCallBackFunc();
                mTimer = null;
            }
        }

        #endregion

        #region Function

        private TimerEvent tmpEvent = null;

        // Update is called once per frame
        void Update()
        {
            for (int i = EventLists.Count - 1; i >= 0; --i)
            {
                tmpEvent = EventLists[i];
                try
                {
                    if (tmpEvent.IsCanRemoveEvent())
                    {
                        tmpEvent.IsFinish = true;
                    }
                }
                catch (Exception exp)
                {
                    Debug.LogException(exp);
                    tmpEvent.IsFinish = true;
                }
            }

            for (int i = EventLists.Count - 1; i >= 0; --i)
            {
                tmpEvent = EventLists[i];
                if (tmpEvent.IsFinish)
                {
                    try
                    {
                        if (tmpEvent.OnFinish != null)
                            tmpEvent.OnFinish();
                    }
                    catch (Exception exp)
                    {
                        Debug.LogException(exp);
                    }
                    finally
                    {
                        EventLists.Remove(tmpEvent);
                    }
                }
            }
        }

        #endregion
    }
}
