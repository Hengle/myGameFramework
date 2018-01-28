/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2017/12/25 00:27:56
** desc:  ��Դ����
*********************************************************************************/

using UnityEngine;
using System.Collections;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Framework
{
    public class ResourceMgr : Singleton<ResourceMgr>
    {
        #region Function

        /// <summary>
        /// ������Դ������;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="assetType">��Դ����</param>
        /// <returns>��Դ������;</returns>
        private IAssetLoader<T> CreateLoader<T>(AssetType assetType) where T : Object
        {
            if (assetType == AssetType.Prefab || assetType == AssetType.Model) return new ResLoader<T>();
            return new AssetLoader<T>();
        }

        /// <summary>
        /// AssetBundle����ֱ�Ӽ��ػ�ýű�;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="tempObject">Object</param>
        /// <returns>ctrl</returns>
        public T GetAssetCtrl<T>(Object tempObject) where T : Object
        {
            T ctrl = null;
            GameObject go = tempObject as GameObject;
            if (go != null)
            {
                ctrl = go.GetComponent<T>();
            }
            return ctrl;
        }

        #endregion

        #region Asset Init

        public void InitShader()
        {
            //Shader��ʼ��;
            AssetBundle shaderAssetBundle = AssetBundleMgr.Instance.LoadShaderAssetBundle();
            if (shaderAssetBundle != null)
            {
                shaderAssetBundle.LoadAllAssets();
                Shader.WarmupAllShaders();
                LogUtil.LogUtility.Print("[ResourceMgr]Load Shader and WarmupAllShaders Success!");
            }
            else
            {
                LogUtil.LogUtility.PrintError("[ResourceMgr]Load Shader and WarmupAllShaders failure!");
            }
            //AssetBundleMgr.Instance.UnloadMirroring(AssetType.Shader, "Shader");
        }

        public void InitLua()
        {

        }

        #endregion

        #region Resources Load

        /// <summary>
        /// Resourceͬ������;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>ctrl</returns>
        public T LoadResSync<T>(AssetType type, string assetName) where T : Object
        {
            string path = FilePathUtility.GetResourcePath(type, assetName);
            IAssetLoader<T> loader = CreateLoader<T>(type);
            if (path != null)
            {
                T ctrl = Resources.Load<T>(path);
                if (ctrl != null)
                {
                    return loader.GetAsset(ctrl);
                }
            }
            LogUtil.LogUtility.PrintError(string.Format("[ResourceMgr]LoadResSync Load Asset {0} failure!", assetName + "." + type.ToString()));
            return null;
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns></returns>
        public IEnumerator LoadResAsync<T>(AssetType type, string assetName) where T : Object
        {
            IEnumerator itor = LoadResAsync<T>(type, assetName, null, null);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <returns></returns>
        public IEnumerator LoadResAsync<T>(AssetType type, string assetName, Action<T> action) where T : Object
        {
            IEnumerator itor = LoadResAsync<T>(type, assetName, action, null);
            while (itor.MoveNext())
            {
                yield return null;
            }
        }

        /// <summary>
        /// Resource�첽����;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">���Ȼص�</param>
        /// <returns></returns>
        public IEnumerator LoadResAsync<T>(AssetType type, string assetName, Action<T> action, Action<float> progress) where T : Object
        {
            string path = FilePathUtility.GetResourcePath(type, assetName);
            IAssetLoader<T> loader = CreateLoader<T>(type);

            T ctrl = null;
            if (path != null)
            {
                ResourceRequest request = Resources.LoadAsync<T>(path);
                while (request.progress < 0.99)
                {
                    if (progress != null) progress(request.progress);
                    yield return null;
                }
                while (!request.isDone)
                {
                    yield return null;
                }
                ctrl = loader.GetAsset(request.asset as T);
            }
            if (action != null)
            {
                action(ctrl);
            }
            else
            {
                LogUtil.LogUtility.PrintError(string.Format("[ResourceMgr]LoadResAsync Load Asset {0} failure!", assetName + "." + type.ToString()));
            }
        }

        #endregion

        #region AssetBundle Load

        /// <summary>
        /// Asset sync load from AssetBundle;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>ctrl</returns>
        public T LoadAssetFromAssetBundleSync<T>(AssetType type, string assetName) where T : Object
        {
            T ctrl = null;
            IAssetLoader<T> loader = CreateLoader<T>(type);

            AssetBundle assetBundle = AssetBundleMgr.Instance.LoadAssetBundleSync(type, assetName);
            if (assetBundle != null)
            {
                T tempObject = assetBundle.LoadAsset<T>(assetName);
                ctrl = loader.GetAsset(tempObject);
            }
            if (ctrl == null)
                LogUtil.LogUtility.PrintError(string.Format("[ResourceMgr]LoadAssetFromAssetBundleSync Load Asset {0} failure!", assetName + "." + type.ToString()));
            return ctrl;
        }

        /// <summary>
        /// Asset async load from AssetBundle;
        /// </summary>
        /// <typeparam name="T">ctrl</typeparam>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <param name="action">��Դ�ص�</param>
        /// <param name="progress">progress�ص�</param>
        /// <returns></returns>
        public IEnumerator LoadAssetFromAssetBundleAsync<T>(AssetType type, string assetName, Action<T> action, Action<float> progress)
            where T : Object
        {
            T ctrl = null;
            AssetBundle assetBundle = null;
            IAssetLoader<T> loader = CreateLoader<T>(type);

            IEnumerator itor = AssetBundleMgr.Instance.LoadAssetBundleAsync(type, assetName,
                ab =>
                {
                    assetBundle = ab;
                },
                null);
            while (itor.MoveNext())
            {
                yield return null;
            }

            AssetBundleRequest request = assetBundle.LoadAssetAsync<T>(assetName);
            while (request.progress < 0.99)
            {
                if (progress != null)
                    progress(request.progress);
                yield return null;
            }
            while (!request.isDone)
            {
                yield return null;
            }
            ctrl = loader.GetAsset(request.asset as T);
            if (ctrl == null)
                LogUtil.LogUtility.PrintError(string.Format("[ResourceMgr]LoadAssetFromAssetBundleSync Load Asset {0} failure!", assetName + "." + type.ToString()));
            if (action != null)
                action(ctrl);
        }

        #endregion

    }
}
