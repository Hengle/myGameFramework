/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/10/07 15:50:22
** desc:  �汾������Ϣ;
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public struct Version
    {
        public Version(int num)
        {
            _apkVersion = 0;
            _hotVersion = 0;
            _versionTime = 0;
            _apkPermissions = 0;
        }

        public int _apkVersion;
        public int _hotVersion;
        public int _versionTime;
        public int _apkPermissions;
    }

    public class VersionInfo
    {
        public string _updateUrl = "";

        //�汾��:xxx.xxx.xxx.xxx:��汾.�ȸ��汾.ʱ��.��/��汾;
        //1.3.181008.0:��һ����汾,�������ȸ��汾,18��10��8�ų��İ汾,ż��Ϊ�ڲ��汾;
        public Version _version;
    }
}