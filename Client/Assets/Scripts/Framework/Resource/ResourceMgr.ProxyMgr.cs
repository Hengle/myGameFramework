/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/09 22:54:09
** desc:  ��Դ���ش���;
*********************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MEC;
using Object = UnityEngine.Object;

namespace Framework
{
    public partial class ResourceMgr
    {
        #region Proxy

        public ManagerInitEventHandler ResourceMgrInitHandler = null;

        private List<AsyncProxy> _removexyList = new List<AsyncProxy>();

        /// <summary>
        /// �Ŷ�ɾ����û�������ɾ����Proxy;
        /// </summary>
        private void UpdateProxy()
        {
            var count = _removexyList.Count;
            for (int i = count - 1; i >= 0; i--)//�������ɾ��;
            {
                var target = _removexyList[i];
                if (target.UnloadProxy())
                {
                    _removexyList.Remove(target);
                }
            }
        }

        public void AddProxy(AsyncProxy proxy)
        {
            _removexyList.Add(proxy);
        }

        /// <summary>
        /// �ȴ�������ɾ��;
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CancleAllProxy()
        {
            yield return Timing.WaitForOneFrame;
            while (true)
            {
                if (_removexyList.Count > 0)
                {
                    yield return Timing.WaitForOneFrame;
                }
                else
                {
                    if (ResourceMgrInitHandler != null)
                    {
                        ResourceMgrInitHandler();
                    }
                    break;
                }
            }
        }

        #endregion
    }
}
