using Microsoft.CodeAnalysis;
using Natasha.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha.Complier
{
    public  abstract partial class IComplier
    {

        public readonly CompilationException Exception;
        public AssemblyDomain Domain;

        public IComplier() => Exception = new CompilationException();




        /// <summary>
        /// 编译时错误提示处理
        /// </summary>
        /// <param name="msg"></param>
        public virtual void SingleError(Diagnostic msg)
        {
            Exception.Diagnostics.Add(msg);
        }




        /// <summary>
        /// 语法树结果检测
        /// </summary>
        /// <param name="source">原脚本字符串</param>
        /// <param name="formart">格式化后的脚本字符串</param>
        /// <param name="errors">错误信息集合</param>
        /// <returns></returns>
        public bool CheckSyntax(string source, string formart, IEnumerable<Diagnostic> errors)
        {

            Exception.Source = source;
            Exception.Formatter = formart;
            Exception.Diagnostics.AddRange(errors);


            if (Exception.Diagnostics.Count != 0)
            {

                Exception.ErrorFlag = ComplieError.Syntax;
                Exception.Message = "语法错误,请仔细检查脚本代码！";
                return false;

            }
            return true;

        }




        /// <summary>
        /// 获取编译后的程序集
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <returns></returns>
        public Assembly GetAssemblyByScript(string content)
        {

            if (Domain == null)
            {
                Domain = AssemblyManagment.Default;
            }


            Assembly assembly =null ;
           var treeResult = GetTreeInfo(content);

            if (CheckSyntax(content, treeResult.Formatter, treeResult.Errors))
            {
                if (Exception.Diagnostics.Count != 0)
                {

                    Exception.ErrorFlag = ComplieError.Syntax;
                    Exception.Message = "发生错误,无法生成程序集！";

                }
                else
                {

                    var result = StreamComplier(treeResult.TypeNames[0], treeResult.Tree, Domain);
                    assembly = result.Assembly;
                    if (assembly == default || assembly == null)
                    {

                        Exception.Diagnostics.AddRange(result.Errors);
                        Exception.ErrorFlag = ComplieError.Assembly;
                        Exception.Message = "发生错误,无法生成程序集！";


                        if (NError.Enabled)
                        {

                            NError logError = new NError();
                            logError.WrapperCode(Exception.Formatter);
                            logError.Handler(result.Compilation, Exception.Diagnostics);
                            logError.Write();
                        }

                    }
                    else if (NSucceed.Enabled)
                    {

                        NSucceed logSucceed = new NSucceed();
                        logSucceed.WrapperCode(Exception.Formatter);
                        logSucceed.Handler(result.Compilation, assembly);

                    }

                }
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

            var type = assembly.GetTypes().First(item => item.Name == typeName);
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
        public MethodInfo GetMethodByScript(string content, string typeName, string methodName = null)
        {

            var type = GetTypeByScript(content, typeName);
            if (type == null)
            {
                return null;
            }


            if (methodName == null)
            {
                methodName = ScriptHelper.GetMethodName(content);
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
