using Microsoft.CodeAnalysis;
using Natasha.Complier;
using System;
using System.Reflection;

namespace Natasha
{
    public abstract class IComplier
    {

        public readonly CompilationException Exception;


        public IComplier() => Exception = new CompilationException();




        /// <summary>
        /// 编译时错误提示处理
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SingleError(Diagnostic msg)
        {
            Exception.Diagnostics.Add(msg);
        }




        protected bool _useFileComplie;
        /// <summary>
        /// 是否启用文件编译，默认不启用
        /// </summary>
        /// <param name="shut">开关</param>
        /// <returns></returns>
        public IComplier UseFileComplie()
        {

            _useFileComplie = true;
            return this;

        }
        public IComplier UseMemoryComplie()
        {

            _useFileComplie = false;
            return this;

        }





        /// <summary>
        /// 获取编译后的程序集
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <returns></returns>
        public Assembly GetAssemblyByScript(string content)
        {

            Exception.Source = content;
            Assembly assembly;
            if (!_useFileComplie)
            {

                assembly = ScriptComplierEngine.StreamComplier(content, out Exception.Formatter, SingleError);

            }
            else
            {

                assembly = ScriptComplierEngine.FileComplier(content, out Exception.Formatter, SingleError);

            }


            //判空
            if (assembly == default)
            {

                Exception.ErrorFlag = ComplieError.Assembly;
                Exception.Message = "发生错误,无法生成程序集！";

            }


            return assembly;

        }




        /// <summary>
        /// 获取编译后的类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public Type GetTypeByScript(string content, string typeName)
        {
            Assembly assembly = GetAssemblyByScript(content);

            if (assembly == null)
            {
                return null;
            }

            var type = AssemblyOperator.Loader(assembly)[typeName];
            if (type == null)
            {

                Exception.ErrorFlag = ComplieError.Type;
                Exception.Message = $"发生错误,无法从程序集{assembly.FullName}中获取类型{typeName}！";

            }

            return type;

        }




        /// <summary>
        /// 获取编译后的方法元数据
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public MethodInfo GetMethodByScript(string content, string typeName, string methodName)
        {

            var type = GetTypeByScript(content, typeName);
            if (type == null)
            {
                return null;
            }


            var info = type.GetMethod(methodName);
            if (info == null)
            {

                Exception.ErrorFlag = ComplieError.Method;
                Exception.Message = $"发生错误,无法从类型{typeName}中找到{methodName}方法！";


            }


            return info;

        }




        /// <summary>
        /// 获取编译后的委托
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate GetDelegateByScript(string content, string typeName, string methodName, Type delegateType, object binder = null)
        {

            var info = GetMethodByScript(content, typeName, methodName);
            if (info == null)
            {
                return null;
            }



            try
            {

                return info.CreateDelegate(delegateType, binder);

            }
            catch (Exception ex)
            {

                Exception.ErrorFlag = ComplieError.Delegate;
                Exception.Message = $"发生错误,无法从方法{methodName}创建{delegateType.GetDevelopName()}委托！";

            }


            return null;

        }


        public T GetDelegateByScript<T>(string content, string typeName, string methodName, object binder = null) where T : Delegate
        {

            return (T)GetDelegateByScript(content, typeName, methodName, typeof(T));

        }

    }

}
