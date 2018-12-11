/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/11 23:34:41
** desc:  �첽���ع���;
*********************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public partial class ResourceMgr
    {
        private class AsyncMgr
        {
            private static float _curStartTime = 0f;

            private static ulong _loadID = ulong.MaxValue;
            public static ulong LoadID
            {
                get
                {
                    if (_loadID < 1)
                        _loadID = ulong.MaxValue;
                    return _loadID--;
                }
            }

            private static ulong _curLoadID = 0;
            public static ulong CurLoadID
            {
                get
                {
                    if (_curLoadID > 0)
                    {
                        return _curLoadID;
                    }
                    if (_curLoadID == 0)
                    {
                        if (AsyncLoadIdList.Count == 0)
                        {
                            LogHelper.PrintError("[ResourceMgr]Get CurLoadID error!");
                            _curLoadID = 0;
                        }
                        _curLoadID = AsyncLoadIdList[0];
                        _curStartTime = GetCurTime();
                    }
                    return _curLoadID;
                }
                set
                {
                    if (0 == value)
                    {
                        Remove(_curLoadID);
                        _curLoadID = 0;
                    }
                }
            }

            //ά��������ԴID����,����������ɵĻص�ִ�е��Ⱥ�˳���ֻ��ű����ýӿڵ�˳���й���;
            private static List<ulong> AsyncLoadIdList = new List<ulong>();

            //ͬʱ���������;
            public static readonly int ASYNC_LOAD_MAX_VALUE = 50;
            //������ɺ�ȴ�ʱ��;
            public static readonly float ASYNC_WAIT_MAX_DURATION = 500f;

            /// <summary>
            /// ���;
            /// </summary>
            /// <param name="id"></param>
            public static void Add(ulong id)
            {
                AsyncLoadIdList.Add(id);
            }

            /// <summary>
            /// ɾ��;
            /// </summary>
            /// <param name="id"></param>
            public static void Remove(ulong id)
            {
                AsyncLoadIdList.Remove(id);
            }

            /// <summary>
            /// ��ǰ����;
            /// </summary>
            /// <returns></returns>
            public static int CurCount()
            {
                return AsyncLoadIdList.Count;
            }

            /// <summary>
            /// �Ƿ��ڼ��ض���;
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static bool IsContains(ulong id)
            {
                return AsyncLoadIdList.Contains(id);
            }

            /// <summary>
            /// ��ǰʱ��;
            /// </summary>
            /// <returns></returns>
            public static float GetCurTime()
            {
                return Time.realtimeSinceStartup;
            }

            /// <summary>
            /// �Ƿ�ʱ;
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static bool IsTimeOverflows(float time)
            {
                return (Time.realtimeSinceStartup - time) > ASYNC_WAIT_MAX_DURATION;
            }

            /// <summary>
            /// ��ǰ�����Ƿ�ʱ;
            /// </summary>
            /// <returns></returns>
            public static bool CurLoadTimeOverflows()
            {
                return IsTimeOverflows(_curStartTime);
            }
        }
    }
}
