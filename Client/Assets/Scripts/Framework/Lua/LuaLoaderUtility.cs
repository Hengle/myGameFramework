/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2017/12/28 00:32:46
** desc:  Lua���ع���
*********************************************************************************/

using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class LuaLoaderUtility : LuaFileUtils
    {
        /// <summary>
        /// ���캯��;
        /// </summary>
        /// <param name="isBundles">�Ƿ�ʹ��AssetBundle����</param>
        public LuaLoaderUtility(bool isBundles = false)
        {
            beZip = isBundles;
        }

        public void AddBundle(string bundleName)
        {

        }
    }
}
