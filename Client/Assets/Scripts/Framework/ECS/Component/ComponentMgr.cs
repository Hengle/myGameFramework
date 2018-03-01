/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/01/14 14:16:25
** desc:  �������
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LogUtil;
using System;
using MEC;

namespace Framework
{
    public class ComponentMgr : MonoSingleton<ComponentMgr>
    {
        public override void Init()
        {
            base.Init();
            //...
        }

        #region Field

        private Dictionary<long, AbsComponent> ComponentDict = new Dictionary<long, AbsComponent>();

        private List<AbsComponent> ComponentList = new List<AbsComponent>();

        #endregion

        #region Unity api

        public override void AwakeEx()
        {
            base.AwakeEx();
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].IsLoaded && ComponentList[i].Enable)
                {
                    ComponentList[i].AwakeEx();
                }
            }
        }

        public override void UpdateEx()
        {
            base.UpdateEx();
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].IsLoaded && ComponentList[i].Enable)
                {
                    ComponentList[i].UpdateEx();
                }
            }
        }

        public override void LateUpdateEx()
        {
            base.LateUpdateEx();
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].IsLoaded && ComponentList[i].Enable)
                {
                    ComponentList[i].LateUpdateEx();
                }
            }
        }

        public override void OnDestroyEx()
        {
            base.OnDestroyEx();
            for (int i = 0; i < ComponentList.Count; i++)
            {
                if (ComponentList[i].IsLoaded && ComponentList[i].Enable)
                {
                    ComponentList[i].OnDestroyEx();
                }
            }
        }

        #endregion

        #region Function

        /// <summary>
        /// ����Component;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="initCallBack">������ɻص�</param>
        /// <param name="isUsePool"></param>
        /// <returns></returns>
        public T CreateComponent<T>(AbsEntity entity, Action<AbsComponent> initCallBack,
            bool isUsePool = false) where T : AbsComponent, new()
        {
            T _Component = new T();
            if (AddComponent(_Component))
            {
                _Component.InitCallBack = initCallBack;
                _Component.OnInit(entity, isUsePool);
                return _Component;
            }
            else
            {
                LogUtil.LogUtility.PrintError("[ComponentMgr]CreateComponent " + typeof(T).ToString() + " error!");
                return null;
            }
        }

        /// <summary>
        /// ���Component;
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private bool AddComponent(AbsComponent component)
        {
            if (ComponentDict.ContainsKey(component.ID))
            {
                return false;
            }
            ComponentDict[component.ID] = component;
            ComponentList.Add(component);
            return true;
        }

        /// <summary>
        /// �Ƴ�Component;
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private bool RemoveComponent(AbsComponent component)
        {
            if (!ComponentDict.ContainsKey(component.ID))
            {
                return false;
            }
            ComponentDict.Remove(component.ID);
            ComponentList.Remove(component);
            return true;
        }

        #endregion
    }
}
