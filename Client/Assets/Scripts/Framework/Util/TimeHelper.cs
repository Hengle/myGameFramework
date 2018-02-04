/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/02/05 01:29:56
** desc:  ʱ�乤��
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public static class TimeHelper
    {
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        /// <summary>
        /// �ͻ���ʱ��;
        /// </summary>
        /// <returns></returns>
        public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000;
        }

        public static long ClientNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000000;
        }

        /// <summary>
        /// ��½ǰ�ǿͻ���ʱ��,��½����ͬ�����ķ�����ʱ��;
        /// </summary>
        /// <returns></returns>
        public static long Now()
        {
            return ClientNow();
        }
    }
}