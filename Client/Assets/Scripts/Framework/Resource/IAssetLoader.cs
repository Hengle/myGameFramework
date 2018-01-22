/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/23 00:39:09
** desc:  Asset���ؽӿ�
*********************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

namespace Framework
{
    /// <summary>
    /// ��Դ���ؽӿ�,��Ҫ�����Ƿ�ʵ����;
    /// </summary>
    public interface IAssetLoader<T> where T : Object
    {
        T GetAsset(T t);
    }

    public class AssetLoader<T> : IAssetLoader<T> where T : Object
    {
        public T GetAsset(T t)
        {
            return t;
        }
    }

    public class ResLoader<T> : IAssetLoader<T> where T : Object
    {
        public T GetAsset(T t)
        {
            if (t == null) return null;
            return Object.Instantiate(t) as T;
        }
    }
}
