/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/21 14:09:40
** desc:  �ļ�·��������
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework
{
    public static class FilePathUtility
    {
        //AssetBundle����洢·��;
        private static string assetBundlePath = Application.dataPath + "/StreamingAssets/AssetBundle";
        public static string AssetBundlePath
        {
            get { return assetBundlePath; }
        }

        /// <summary>
        /// ��Ҫ�������Դ���ڵ�Ŀ¼;
        /// </summary>
        public static string resPath = "Assets/Bundles/";

        /// <summary>
        /// ��Ҫ�����lua�ļ�;
        /// </summary>
        public static string luaPath = "Assets/Resources/Lua";

        /// <summary>
        /// ��Ҫ�����Atlas�ļ�;
        /// </summary>
        public static string atlasPath = "Assets/Atlas";

        /// <summary>
        /// ��Ҫ�����Scene�ļ�;
        /// </summary>
        public static string scenePath = "Assets/Scenes/";

        /// <summary>
        /// ��·���µ���Դ�������,��Ҫ��Ϊ�˷���ʹ����Դ,��ͼ��,����,������ı�����ͼ�ȵ�;
        /// </summary>
        public static string singleResPath = "Assets/Bundles/Single/";

        /// <summary>
        /// ��ȡAssetBundle�ļ�������;
        /// </summary>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>AssetBundle��Դ����</returns>
        public static string GetAssetBundleFileName(AssetType type, string assetName)
        {
            string assetBundleName = null;

            if (type == AssetType.Non || string.IsNullOrEmpty(assetName)) return assetBundleName;
            //AssetBundle�����ֲ�֧�ִ�д;
            //AssetBundle���������ʽΪ[assetType/assetName.assetbundle],����ʱͬ����Դ������������ͬ,һ��ͬһ�ļ����²����ظ�,ÿ��
            //�ļ����µ���Դ��������ͬ��ǰ׺,��ͬ�ļ�����,��Դǰ׺��ͬ;
            assetBundleName = (type.ToString() + "/" + assetName + ".assetbundle").ToLower();
            return assetBundleName;
        }

        /// <summary>
        /// ��ȡAssetBundle�ļ�����·��;
        /// </summary>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>AssetBundle��Դ·��</returns>
        public static string GetAssetBundlePath(AssetType type, string assetName)
        {
            string assetBundleName = GetAssetBundleFileName(type, assetName);
            if (string.IsNullOrEmpty(assetBundleName)) return null;
            return assetBundlePath + assetBundleName;
        }

        /// <summary>
        /// ��ȡResource�ļ�����·��;
        /// </summary>
        /// <param name="type">��Դ����</param>
        /// <param name="assetName">��Դ����</param>
        /// <returns>Resource��Դ·��;</returns>
        public static string GetResourcePath(AssetType type, string assetName)
        {
            if (type == AssetType.Non || type == AssetType.Scripts || string.IsNullOrEmpty(assetName)) return null;
            string assetPath = null;
            switch (type)
            {
                case AssetType.Prefab: assetPath = "Prefab/"; break;
                default:
                    assetPath = type.ToString() + "/";
                    break;
            }
            assetPath = assetPath + assetName;
            return assetPath;
        }
    }
}
