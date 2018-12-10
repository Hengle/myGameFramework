/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/09 16:12:18
** desc:  AssetBundle��Դ����;
*********************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MEC;
using Object = UnityEngine.Object;
using System.IO;

namespace Framework
{
    public partial class ResourceMgr
    {
        #region AssetBundle Load

        private ulong _loadID = ulong.MaxValue;

        private ulong LoadID
        {
            get
            {
                if (_loadID < 1)
                    _loadID = ulong.MaxValue;
                return _loadID--;
            }
        }

        private ulong _curLoadID = 0;
        private ulong CurLoadID
        {
            get
            {
                if (_curLoadID > 0)
                {
                    return _curLoadID;
                }
                if (_curLoadID == 0)
                {
                    if (_loadIdQueue.Count == 0)
                    {
                        LogHelper.PrintError("[ResourceMgr]Get CurLoadTask error!");
                        _curLoadID = 0;
                    }
                    _curLoadID = _loadIdQueue.Dequeue();
                }
                return _curLoadID;
            }
            set
            {
                if (0 == value)
                {
                    _curLoadID = 0;
                }
            }
        }

        //ά��������ԴID����,��֤��Դ����˳��,����������ɵĻص�ִ�е��Ⱥ�˳���ֻ��ű����ýӿڵ�˳���й���;
        private Queue<ulong> _loadIdQueue = new Queue<ulong>();

        private int LOAD_INTERVAL_TIME = 1000;//���س�ʱʱ��1000ms;
        private int LOAD_MAX_VALUE = 50;//ͬʱ���������;

        private int _curLoadCount = 0;

        /// <summary>
        /// Asset sync load from AssetBundle;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>ctrl</returns>
        [Obsolete("Warning,Suggest use 'ResourceMgr.LoadAssetProxy' instead!")]
        public T LoadAssetSync<T>(AssetType assetType, string assetName) where T : Object
        {
            T ctrl = null;
            IAssetLoader<T> loader = CreateLoader<T>(assetType);
            AssetBundle assetBundle = AssetBundleMgr.Instance.LoadAssetBundleSync(assetType, assetName);
            if (assetBundle != null)
            {
                var name = Path.GetFileNameWithoutExtension(assetName);
                T tempObject = assetBundle.LoadAsset<T>(name);
                ctrl = loader.GetAsset(tempObject);
            }
            if (ctrl == null)
            {
                LogHelper.PrintError(string.Format("[ResourceMgr]LoadAssetSync Load Asset {0} failure!", assetName));
            }
            return ctrl;
        }

        /// <summary>
        /// Asset�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>����</returns>
        public AssetLoadProxy LoadAssetProxy<T>(AssetType assetType, string assetName) where T : Object
        {
            return LoadAssetProxy<T>(assetType, assetName, null, null);
        }

        /// <summary>
        /// Asset�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <returns>����</returns>
        public AssetLoadProxy LoadAssetProxy<T>(AssetType assetType, string assetName, Action<T> action) where T : Object
        {
            return LoadAssetProxy<T>(assetType, assetName, action, null);
        }

        /// <summary>
        /// Asset�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">progress�ص�</param>
        /// <returns>����</returns>
        public AssetLoadProxy LoadAssetProxy<T>(AssetType assetType, string assetName
            , Action<T> action, Action<float> progress) where T : Object
        {
            AssetLoadProxy proxy = PoolMgr.Instance.Get<AssetLoadProxy>();
            proxy.InitProxy(assetType, assetName);
            CoroutineMgr.Instance.RunCoroutine(LoadAssetAsync<T>(assetType, assetName, proxy, action, progress));
            return proxy;
        }

        /// <summary>
        /// Asset async load from AssetBundle;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="proxy">����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">progress�ص�</param>
        /// <returns></returns>
        private IEnumerator<float> LoadAssetAsync<T>(AssetType assetType, string assetName, AssetLoadProxy proxy
            , Action<T> action, Action<float> progress)
            where T : Object
        {
            T ctrl = null;
            AssetBundle assetBundle = null;
            IAssetLoader<T> loader = CreateLoader<T>(assetType);

            if(_curLoadCount>LOAD_MAX_VALUE)
                yield return Timing.WaitForOneFrame;

            var loadID = LoadID;
            _loadIdQueue.Enqueue(loadID);

            IEnumerator itor = AssetBundleMgr.Instance.LoadAssetBundleAsync(assetType, assetName,
                ab => { assetBundle = ab; }, progress);//�˴�����ռ90%;
            while (itor.MoveNext())
            {
                yield return Timing.WaitForOneFrame;
            }
            var name = Path.GetFileNameWithoutExtension(assetName);
            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(name);
            //�˴�����ռ10%;
            while (request.progress < 0.99)
            {
                if (progress != null)
                {
                    progress(0.9f + 0.1f * request.progress);
                }
                yield return Timing.WaitForOneFrame;
            }
            while (!request.isDone)
            {
                yield return Timing.WaitForOneFrame;
            }
            ctrl = loader.GetAsset(request.asset as T);
            if (null == ctrl)
            {
                LogHelper.PrintError(string.Format("[ResourceMgr]LoadAssetAsync Load Asset {0} failure!", assetName));
            }
            //�ȵ�һ֡;
            yield return Timing.WaitForOneFrame;
            if (CurLoadID != loadID)
            {
                yield return Timing.WaitForOneFrame;
            }
            else
            {
                if (!proxy.isCancel && action != null)
                {
                    action(ctrl);
                }
                if (proxy != null)
                {
                    proxy.OnFinish(ctrl);
                }
                CurLoadID = 0;
            }
        }

        #endregion
    }
}
