/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2017/12/26 23:42:52
** desc:  AssetBundle�������
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Framework
{
    public static class AssetBundleDefine
    {
        //AssetBundle����洢·��;
        private static string assetBundlePath = Application.dataPath + "/StreamingAssets/AssetBundle";
        public static string AssetBundlePath
        {
            get { return assetBundlePath; }
        }

        /// <summary>
        /// ������Դ·����ȡ��Դ����;
        /// </summary>
        /// <param name="path">��Դ·��</param>
        /// <returns></returns>
        public static AssetType GetAssetType(string path)
        {
            if (null == path) return AssetType.Non;
            switch (Path.GetExtension(path).ToLower())
            {
                case ".anim":
                    return AssetType.AnimeClip;
                case ".controller":
                case ".overridecontroller":
                    return AssetType.AnimeCtrl;
                case ".ogg":
                case ".wav":
                case ".mp3":
                    return AssetType.Audio;
                case ".png":
                case ".bmp":
                case ".tga":
                case ".psd":
                case ".dds":
                case ".jpg":
                    return AssetType.Texture;
                case ".shader":
                    return AssetType.Shader;
                case ".prefab":
                    if (path.Contains("Atlas"))
                    {
                        return AssetType.AssetPrefab;
                    }
                    if (path.Contains("Model"))
                    {
                        return AssetType.Model;
                    }
                    return AssetType.Prefab;
                case ".unity":
                    return AssetType.Scene;
                case ".mat":
                    return AssetType.Material;
                case ".cs":
                    return AssetType.Scripts;
                case ".ttf":
                    return AssetType.Font;
            }
            return AssetType.Non;
        }
    }

    /// <summary>
    /// ��Դ����;
    /// </summary>
    public enum AssetType
    {
        Non,
        Prefab,
        Model,
        Scene,
        Material,
        Scripts,
        Font,
        /// <summary>
        /// ����Ҫʵ�����ĵ�Prefab��Դ,��ͼ��;
        /// </summary>
        AssetPrefab,
        Shader,
        Texture,
        Audio,
        AnimeCtrl,
        AnimeClip
    }
}
