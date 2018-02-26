/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/20 00:13:50
** desc:  ���������ӿ�
*********************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public interface IPool //: IDisposable
    {
        void OnInit(params Object[] args);
        void OnRelease();
        UnityEngine.GameObject OnAddGameObject();

        //void Dispose();��dispose����GameObject
    }
}
