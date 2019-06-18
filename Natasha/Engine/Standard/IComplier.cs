using Microsoft.CodeAnalysis;
using Natasha.Complier;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Natasha
{
    public abstract class IComplier
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
        /// 获取编译后的程序集
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public Assembly GetAssemblyByScript(string content)
        {
            if (!_useFileComplie)
            {
                return ScriptComplieEngine.StreamComplier(content, SingleError);
            }
            else
            {
                return ScriptComplieEngine.FileComplier(content, SingleError);
            }
        }
    }
}
