/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/21 02:20:27
** desc:  AssetBundle�������
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framework
{
	public static class AssetBuildDefine 
    {
        /// <summary>
        /// ���ѡ��,���AssetBundle��ѹ��,ʹ�õ�����ѹ�����ѹ��,�ٽ�ѹ��ɳ��·��,�ȿ��Լ��ٰ���,�ӿ��ȡ�ٶ�,����ռ������̿ռ�;
        /// CompleteAssetsĬ�Ͽ���;CollectDependenciesĬ�Ͽ���;DeterministicAssetBundleĬ�Ͽ���;ChunkBasedCompressionʹ��LZ4ѹ��;
        /// </summary>
        public static BuildAssetBundleOptions options = BuildAssetBundleOptions.UncompressedAssetBundle;

        /// <summary>
        /// AssetBundle���Ŀ��ƽ̨;
        /// </summary>
        public static BuildTarget buildTarget =

#if UNITY_IOS    //unity5.x UNITY_IPHONE����UNITY_IOS
	BuildTarget.iOS;
#elif UNITY_ANDROID
    BuildTarget.Android;
#else
    EditorUserBuildSettings.activeBuildTarget;
#endif

	}
}
