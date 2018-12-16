/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/23 23:54:19
** desc:  ugui源码;
*********************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.ObjectPool
{
    internal class ListPool<T>
    {
        // Object pool to avoid allocations.
        private ObjectPool<List<T>> s_ListPool = new ObjectPool<List<T>>(null, l => l.Clear());

        public List<T> Get()
        {
            return s_ListPool.Get();
        }

        public void Release(List<T> toRelease)
        {
            s_ListPool.Release(toRelease);
        }
    }
}
