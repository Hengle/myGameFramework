/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/09 20:08:35
** desc:  Resource��Դ�첽���ش���;
*********************************************************************************/

using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class AsyncResourceProxy : AssetProxy
    {
        protected override void Unload()
        {
            PoolMgr.Instance.ReleaseCsharpObject<AsyncResourceProxy>(this);
        }

        protected override void Unload2Pool()
        {
            PoolMgr.Instance.ReleaseCsharpObject<AsyncResourceProxy>(this);
        }
    }
}
