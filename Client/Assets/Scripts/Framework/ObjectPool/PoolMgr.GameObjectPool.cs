/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/16 20:34:27
** desc:  GameObject����ع���;
*********************************************************************************/

using Framework.Pool;
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
        private GameObjectPool _gameObjectPool = new GameObjectPool();

        /// <summary>
        /// ��ȡGameObject;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Clone(GameObject go)
        {
            return _gameObjectPool.Clone(go);
        }

        /// <summary>
        /// ����GameObject;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <param name="element"></param>
        public void Release(GameObject element)
        {
            _gameObjectPool.Release(element);
        }
    }
}
