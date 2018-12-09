/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/12/09 20:08:35
** desc:  Resource��Դ���ش���;
*********************************************************************************/

using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourceLoadProxy
    {
        public AssetType assetType { get; private set; }
        public string assetName { get; private set; }
        public bool isFinish { get; private set; }//�Ƿ�������;
        public bool isCancel { get; private set; }//ȡ���˾Ͳ���ִ���첽���صĻص���;
        public Object targetObject { get; private set; }

        /// <summary>
        /// ��ʼ��;
        /// </summary>
        /// <param name="assetType"></param>
        /// <param name="assetName"></param>
        public void InitProxy(AssetType assetType, string assetName)
        {
            this.assetType = assetType;
            this.assetName = assetName;
            isCancel = false;
            isFinish = false;
        }

        public void OnFinish(Object target)
        {
            targetObject = target;
            isFinish = true;
        }

        public void CancelProxy()
        {
            isCancel = true;
            if (!UnloadProxy())
            {
                ResourceMgr.Instance.AddProxy(this);
            }
        }

        public bool UnloadProxy()
        {
            if (isFinish)
            {
                if (targetObject != null)
                {
                    ResourceMgr.Instance.UnloadObject(assetType, targetObject);
                }
                assetType = AssetType.Non;
                assetName = string.Empty;
                isCancel = false;
                isFinish = false;
                return true;
            }
            return false;
        }
    }
}
