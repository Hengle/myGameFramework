/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/16 20:34:27
** desc:  C#����ع���;
*********************************************************************************/

using Framework.Pool;
using System;
using System.Collections.Generic;

namespace Framework
{
    public partial class PoolMgr
    {
        /// <summary>
        /// C# Object Pool;
        /// </summary>
        private Dictionary<Type, Object> _objectPool = new Dictionary<Type, Object>();

        /// <summary>
        /// ��ȡ�����Ŀ�����;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">��ʼ������</param>
        /// <returns></returns>
        public T GetObject<T>(params Object[] args) where T : new()
        {
            ObjectPool<T> pool;
            Object temp;
            if (_objectPool.TryGetValue(typeof(T), out temp))
            {
                pool = temp as ObjectPool<T>;
            }
            else
            {
                pool = CreatePool<T>();
            }
            T t = pool.Get();
            IPool target = t as IPool;
            if (target != null)
                target.OnGet(args);
            return t;
        }

        /// <summary>
        /// �ͷŶ�������;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        public void Release<T>(T type) where T : new()
        {
            IPool target = type as IPool;
            if (target != null)
                target.OnRelease();
            ObjectPool<T> pool;
            Object temp;
            if (_objectPool.TryGetValue(typeof(T), out temp))
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
            if (_objectPool.TryGetValue(typeof(T), out temp))
            {
                return temp as ObjectPool<T>;
            }
            else
            {
                ObjectPool<T> pool = new ObjectPool<T>(null, null);
                _objectPool[typeof(T)] = pool;
                return pool;
            }
        }
    }
}
