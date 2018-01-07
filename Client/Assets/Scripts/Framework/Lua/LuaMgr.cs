/********************************************************************************
** auth:  https://github.com/HushengStudent
** date:  2017/12/25 00:30:25
** desc:  Lua����
*********************************************************************************/

using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    //�ο�(https://github.com/jarjin/LuaFramework_UGUI)����tolua;
    public class LuaMgr : MonoSingleton<LuaMgr> 
	{
        private LuaState lua;
        private LuaLoaderUtility loader;
        private LuaLooper loop = null;

        public override void Init()
        {
            base.Init();
            LogUtility.Print("[LuaMgr]Init!", LogColor.Green);
        }

        public override void AwakeEx()
        {
            base.AwakeEx();
            //��ʼ��LuaMgr;
            loader = new LuaLoaderUtility();//TODO:Lua AssetBundle��ʹ��;
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);
            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
        }
        
        /// <summary>
        /// ��ʼ�����ص�������;
        /// </summary>
        void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            //lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            //lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);
            this.OpenCJson();
        }

        //cjson�Ƚ�����,ֻnew��һ��table,û��ע���,����ע��һ��;
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");
            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        public void StartLuaMgr()
        {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //����LUAVM;
            this.StartMain();
            this.StartLooper();
        }

        /// <summary>
        /// ��ʼ��Lua�������·��;
        /// </summary>
        void InitLuaPath()
        {
            lua.AddSearchPath(LuaConst.luaDir);
            lua.AddSearchPath(LuaConst.luaResDir);
        }

        /// <summary>
        /// ��ʼ��LuaBundle;
        /// </summary>
        void InitLuaBundle()
        {
            if (loader.beZip)
            {
                //loader.AddBundle("lua/lua.unity3d");
            }
        }

        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        void StartMain()
        {
            lua.DoFile("Main.lua");
            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;
        }

        public void DoFile(string filename)
        {
            lua.DoFile(filename);
        }

        // Update is called once per frame;
        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                return func.LazyCall(args);
            }
            return null;
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }
	}
}
