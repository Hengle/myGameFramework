/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/10 01:03:13
** desc:  AssetBundle��Դ���ش���;
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Framework
{
    public class AssetLoadProxy : AbsLoadProxy
    {
        protected override void Unload()
        {
            if (targetObject != null)
            {
                ResourceMgr.Instance.UnloadObject(assetType, targetObject);
                //ж��AssetBundle;
                AssetBundleMgr.Instance.UnloadAsset(assetType, assetName);
            }
            PoolMgr.Instance.Release<AssetLoadProxy>(this);
        }
    }
}
