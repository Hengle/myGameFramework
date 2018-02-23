/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/20 00:13:50
** desc:  ���������ӿ�
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public interface IPool
    {
        void Init(params Object[] args);
        void Release();
    }
}
