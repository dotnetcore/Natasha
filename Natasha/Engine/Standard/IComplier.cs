using Microsoft.CodeAnalysis;
using Natasha.Complier;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Natasha
{
    public abstract class IComplier : IScriptBuilder
    {
        /// <summary>
        /// 编译时错误提示处理
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SingleError(Diagnostic msg)
        {
#if DEBUG
            Debug.WriteLine(msg.GetMessage());
#endif
        }




        protected bool _useFileComplie;
        /// <summary>
        /// 是否启用文件编译，默认不启用
        /// </summary>
        /// <param name="shut">开关</param>
        /// <returns></returns>
        public IComplier UseFileComplie(bool shut = true)
        {
            _useFileComplie = shut;
            return this;
        }




        /// <summary>
        /// 生成委托
        /// </summary>
        /// <returns></returns>
        public Delegate Create()
        {
            return Complie();
        }




        /// <summary>
        /// 生成强类型委托
        /// </summary>
        /// <typeparam name="T">强类型</typeparam>
        /// <returns></returns>
        public T Create<T>() where T: Delegate
        {
            return (T)Complie();
        }




        /// <summary>
        /// 获取编译后的程序集
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public Assembly GetAssemblyByScript(string className=null)
        {
            if (!_useFileComplie)
            {
                return ScriptComplier.StreamComplier(Builder(), className, SingleError);
            }
            else
            {
                return ScriptComplier.FileComplier(Builder(), className, SingleError);
            }
        }




        /// <summary>
        /// 编译虚方法
        /// </summary>
        /// <returns></returns>
        public virtual Delegate Complie()
        {
            return null;
        }




        /// <summary>
        /// 构建脚本，虚方法
        /// </summary>
        /// <returns></returns>
        public virtual string Builder()
        {
            return "";
        }
    }
}
