/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/14 18:14:29
** desc:  ����ع���
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Framework
{
    public class PoolMgr : Singleton<PoolMgr>
    {
        /// <summary>
        /// C# Object Pool;
        /// </summary>
        private Dictionary<Type, Object> _pool = new Dictionary<Type, Object>();

        /// <summary>
        /// GameObject Pool;
        /// </summary>
        private GameObjectPool _gameObjectPool = new GameObjectPool();

        /// <summary>
        /// ��ȡ�����Ŀ�����;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">��ʼ������</param>
        /// <returns></returns>
        public T Get<T>(params Object[] args) where T : IPool, new()
        {
            ObjectPool<T> pool;
            Object temp;
            if (_pool.TryGetValue(typeof(T), out temp))
            {
                pool = temp as ObjectPool<T>;
            }
            else
            {
                pool = CreatePool<T>();
            }
            T t = pool.Get();
            t.OnGet(args);
            return t;
        }

        /// <summary>
        /// �ͷŶ�������;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void Release<T>(T type) where T : IPool, new()
        {
            type.OnRelease();
            ObjectPool<T> pool;
            Object temp;
            if (_pool.TryGetValue(typeof(T), out temp))
            {
                pool = temp as ObjectPool<T>;
            }
            else
            {
                pool = CreatePool<T>();
            }
            pool.Release(type);
        }

        /// <summary>
        /// ���������;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private ObjectPool<T> CreatePool<T>() where T : new()
        {
            Object temp;
            if (_pool.TryGetValue(typeof(T), out temp))
            {
                return temp as ObjectPool<T>;
            }
            else
            {
                ObjectPool<T> pool = new ObjectPool<T>(null, null);
                _pool[typeof(T)] = pool;
                return pool;
            }
        }

        /// <summary>
        /// ��ȡGameObject,����Ϊnull;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public GameObject Get(AssetType type, string assetName)
        {
            return _gameObjectPool.Get(type, assetName);
        }

        /// <summary>
        /// ����GameObject;
        /// </summary>
        /// <param name="type"></param>
        /// <param name="assetName"></param>
        /// <param name="element"></param>
        public void Release(AssetType type, string assetName, GameObject element)
        {
            _gameObjectPool.Release(type, assetName, element);
        }

        /// <summary>
        /// ���ٶ����;
        /// </summary>
        public void ClearPool(Action onFinish)
        {
            _pool.Clear();
            CoroutineMgr.Instance.RunCoroutine(_gameObjectPool.ClearPool(
                () =>
                {
                    System.GC.Collect();
                    if (onFinish != null)
                    {
                        onFinish();
                    }
                }
                ));
        }
    }
}
