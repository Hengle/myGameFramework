/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/02/28 00:50:20
** desc:  Buff
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// ��������;
    /// </summary>
    internal enum NetBufferType
    {
        _4K,
        _32K,
    }

    public class NetBuffer32K : NetBuffer
    {
        public NetBuffer32K()
        {
            m_buffer = new byte[1024 * 32];
            BuffSizeType = NetBufferType._32K;
        }
    }

    public class NetBuffer
    {
        public NetBuffer()
        {
            m_buffer = new byte[1024 * 4];
            BuffSizeType = NetBufferType._4K;
        }

        internal NetBufferType BuffSizeType { get; set; }

        /// <summary>
        /// �ֽڵ�����;
        /// </summary>
        protected byte[] m_buffer;

        public Byte[] Bytes
        {
            get { return m_buffer; }
        }

        /// <summary>
        /// ��ǰʹ�õ����ݳ���;
        /// </summary>
        public int Length { get; set; }

        private int referenceCounter;

        /// <summary>
        /// ���󻺳����ݵĴ�С(ע�⣬�����Byte���ص���������ý���ͬ);
        /// </summary>
        /// <param name="minSize">�������С�ߴ�</param>
        public void UpdateCapacity(int minSize = 0)
        {
            int newSize;
            if (minSize == 0)
            {
                newSize = m_buffer.Length * 2;
            }
            else
            {
                newSize = FixSize(minSize);
                LogUtil.LogUtility.Print(string.Format("[NetBuff]UpdateCapacity size={0} newsize={1}", minSize, newSize));
            }
            var newBuffer = new byte[newSize];
            Buffer.BlockCopy(m_buffer, 0, newBuffer, 0, m_buffer.Length);
            m_buffer = newBuffer;
        }

        /// <summary>
        /// ����4K����;
        /// </summary>
        /// <param name="minSize"></param>
        /// <returns></returns>
        private int FixSize(int minSize)
        {
            return (minSize / 4096 + 1) * 4096;
        }

        /// <summary>
        /// ���������ͨ��Pool��ö���,���õ��ø÷���;
        /// ����ǲ�������,������Ҫʹ������byte����,����Ҫ��Use,��Release;
        /// </summary>
        public void Use()
        {
            referenceCounter++;
        }

        /// <summary>
        /// ������ʹ�û�����ʱ,��Ҫ�ֶ��ͷ�,������Զ����ض������;
        /// </summary>
        public void Release()
        {
            referenceCounter--;
            if (referenceCounter < 0)
            {
                LogUtil.LogUtility.Print("[NetBuff]Release repeat!");
                referenceCounter = 0;
                return;
            }
            if (referenceCounter == 0)
            {
                //ReleaseToPool(this);
            }
        }
    }
}
