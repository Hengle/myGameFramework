/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/16 20:34:27
** desc:  GameObject����ع���;
*********************************************************************************/

using Framework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public partial class PoolMgr
    {
        /// <summary>
        /// Unity Object Pool;
        /// </summary>
        private UnityGameObjectPool _unityGameObjectPool = new UnityGameObjectPool();

        /// <summary>
        /// ��ȡGameObject;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Clone(GameObject go)
        {
            return _unityGameObjectPool.Clone(go);
        }

        /// <summary>
        /// ����GameObject;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <param name="element"></param>
        public void ReleaseGameObject(GameObject element)
        {
            _unityGameObjectPool.Release(element);
        }
    }
}
