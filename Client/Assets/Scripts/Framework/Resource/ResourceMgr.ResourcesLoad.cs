/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/09 16:12:18
** desc:  Resource��Դ����;
*********************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MEC;
using Object = UnityEngine.Object;

namespace Framework
{
    /// <summary>
    /// ��Resources.LoadAsync���ص�ͬһ֡,��Resources.Load����ͬһ��Դ�ᱨ��;
    /// </summary>
    public partial class ResourceMgr
    {
        #region Resources Load

        /// <summary>
        /// Resourceͬ������;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>ctrl</returns>
        public T LoadResourceSync<T>(AssetType assetType, string assetName) where T : Object
        {
            string path = FilePathHelper.GetResourcePath(assetType, assetName);
            if (path != null)
            {
                //Resources.Load����ͬһ��Դ,ֻ����һ��Asset,��Ҫʵ��������Դ����Instantiate�������;
                T ctrl = Resources.Load<T>(path);
                if (ctrl != null)
                {
                    return ctrl;
                }
            }
            LogHelper.PrintError(string.Format("[ResourceMgr]LoadResourceSync Load Asset {0} failure!",
                assetName + "." + assetType.ToString()));
            return null;
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>����</returns>
        public ResourceAsyncProxy LoadResourceProxy<T>(AssetType assetType, string assetName) where T : Object
        {
            return LoadResourceProxy<T>(assetType, assetName, null, null);
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <returns>����</returns>
        public ResourceAsyncProxy LoadResourceProxy<T>(AssetType assetType, string assetName
            , Action<T> action) where T : Object
        {
            return LoadResourceProxy<T>(assetType, assetName, action, null);
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">progress�ص�</param>
        /// <returns>����</returns>
        public ResourceAsyncProxy LoadResourceProxy<T>(AssetType assetType, string assetName
            , Action<T> action, Action<float> progress) where T : Object
        {
            ResourceAsyncProxy proxy = PoolMgr.Instance.GetObject<ResourceAsyncProxy>();
            proxy.InitProxy(assetType, assetName);
            CoroutineMgr.Instance.RunCoroutine(LoadResourceAsync<T>(assetType, assetName, proxy, action, progress));
            return proxy;
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="proxy">����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">progress�ص�</param>
        /// <returns></returns>
        private IEnumerator<float> LoadResourceAsync<T>(AssetType assetType, string assetName, ResourceAsyncProxy proxy
            , Action<T> action, Action<float> progress) where T : Object
        {
            string path = FilePathHelper.GetResourcePath(assetType, assetName);
            T ctrl = null;
            if (path != null)
            {
                ResourceRequest request = Resources.LoadAsync<T>(path);
                while (request.progress < 0.99)
                {
                    if (progress != null) progress(request.progress);
                    yield return Timing.WaitForOneFrame;
                }
                while (!request.isDone)
                {
                    yield return Timing.WaitForOneFrame;
                }
                ctrl = request.asset as T;
            }
            if (null == ctrl)
            {
                LogHelper.PrintError(string.Format("[ResourceMgr]LoadResourceAsync Load Asset {0} failure!",
                    assetName + "." + assetType.ToString()));
            }
            //--------------------------------------------------------------------------------------
            //�ȵ�һ֡;
            yield return Timing.WaitForOneFrame;

            if (!proxy.isCancel && action != null)
            {
                action(ctrl);
            }
            if (proxy != null)
            {
                proxy.OnFinish(ctrl);
            }
        }

        #endregion
    }
}
