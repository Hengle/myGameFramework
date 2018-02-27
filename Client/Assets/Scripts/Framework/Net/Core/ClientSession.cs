/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/02/28 00:30:26
** desc:  �Ự
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Framework
{
    public class ClientSession<T>
    {
        /// <summary>
        /// �����Ķ���;
        /// </summary>
        internal SocketAsyncEventArgs SendEventArgs { get; private set; }
        internal SocketAsyncEventArgs ReceiveEventArgs { get; private set; }
        internal SocketAsyncEventArgs ConnectEventArgs { get; private set; }

        /// <summary>
        /// ��Session�����Ķ���;
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// �ͻ��˶�Ӧ��Socket;
        /// </summary>
        internal Socket Socket { get; private set; }

        /// <summary>
        /// ���캯��;
        /// </summary>
        public ClientSession()
        {
            Init();
        }

        ~ClientSession()
        {
            LogUtil.LogUtility.Print("[ClientSession]Session release!");
        }

        void Init()
        {
            //SendEventArgs = new SocketAsyncEventArgs();
            //SendEventArgs.Completed += OnSendCompleted;

            //ReceiveEventArgs = new SocketAsyncEventArgs();
            //ReceiveEventArgs.Completed += OnRecvCompleted;

            //ConnectEventArgs = new SocketAsyncEventArgs();
            //ConnectEventArgs.Completed += OnConnectCompleted;
        }
    }
}
