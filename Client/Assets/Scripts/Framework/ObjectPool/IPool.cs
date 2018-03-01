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
        void OnGet(params Object[] args);
        void OnRelease();
        void OnClear();
    }
}
