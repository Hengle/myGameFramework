/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2018/08/20 01:44:34
** desc:  Object��չ;
*********************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class ObjectEx : IPool
    {
        public ObjectEx()
        {
            _id = IdGenerater.GenerateId();
            Enable = false;
        }

        private long _id;
        public long ID { get { return _id; } }
        public bool Enable { get; set; }

        /// <summary>
        /// �����Get;
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnGet(params System.Object[] args) { }
        /// <summary>
        /// �����Release;
        /// </summary>
        public virtual void OnRelease() { }
    }
}