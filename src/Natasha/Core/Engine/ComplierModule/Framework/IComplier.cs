using Microsoft.CodeAnalysis;
using Natasha.Complier.Model;
using Natasha.Log;
using Natasha.Template;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        public readonly List<CompilationException> SyntaxExceptions;
        public readonly CompilationException ComplieException;
        public string AssemblyName;
        public bool ComplieInFile;
        public int ErrorRetryCount;
        public readonly static string CurrentPath;

        static IComplier()
        {

            CurrentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dynamiclib");
            if (!Directory.Exists(CurrentPath))
            {
                Directory.CreateDirectory(CurrentPath);
            }

        }

        public IComplier()
        {

            ComplieException = new CompilationException();
            _domain = DomainManagment.Default;
            References = new List<PortableExecutableReference>(_domain.ReferencesCache);
            SyntaxInfos = new SyntaxOption();
            SyntaxExceptions = SyntaxInfos.SyntaxExceptions;
            //ComplieInFile = true;

        }









        /// <summary>
        /// 获取编译后的程序集
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {

            if (SyntaxInfos.Trees.Count == 0)
            {
                return null;
            }


            if (AssemblyName == default)
            {
                AssemblyName = Guid.NewGuid().ToString("N");
            }


            var result = StreamComplier();
            Assembly assembly = result.Assembly;
            if (result.Compilation != null)
            {

                if (assembly == default || assembly == null)
                {

                    bool CS0104SHUT = false;
                    bool CS0234SHUT = false;
                    bool CSO246SHUT = false;
                    var tempCache = SyntaxInfos.TreeCodeMapping;
                    SyntaxOption option = new SyntaxOption();


                    foreach (var item in result.Errors)
                    {

                        if (item.Id == "CS0104")
                        {

                            CS0104SHUT = true;
                            var tempTree = item.Location.SourceTree;
                            var tempCode = tempCache[tempTree];
                            var (str1, str2) = CS0104Helper.Handler(item.Descriptor.MessageFormat.ToString(), item.GetMessage());
                            var sets = SyntaxInfos.TreeUsingMapping[tempTree];
                            if (sets.Contains(str2))
                            {

                                tempCache[tempTree] = tempCode.Replace($"using {str1};", "");

                            }
                            else if (sets.Contains(str1))
                            {

                                tempCache[tempTree] = tempCode.Replace($"using {str2};", "");

                            }
                            else
                            {

                                tempCache[tempTree] = tempCode.Replace($"using {str1};", "");

                            }

                        }
                        else if (item.Id == "CS0234")
                        {

                            CS0234SHUT = true;
                            var tempResult = CS0234Helper.Handler(item.Descriptor.MessageFormat.ToString(), item.GetMessage());
                            UsingDefaultCache.Remove(tempResult);
                            var tempTree = item.Location.SourceTree;
                            var tempCode = tempCache[tempTree];
                            tempCache[tempTree] = tempCode.Replace($"using {tempResult};", "");

                        }
                        else if (item.Id == "CS0246")
                        {

                            CSO246SHUT = true;
                            var tempTree = item.Location.SourceTree;
                            var tempCode = tempCache[tempTree];
                            var formart = item.Descriptor.MessageFormat.ToString();
                            CS0246Helper.Handler(item.Descriptor.MessageFormat.ToString(), item.GetMessage());
                            foreach (var @using in CS0246Helper.GetUsings(formart, tempCode))
                            {
                                UsingDefaultCache.Remove(@using);
                                tempCache[tempTree] = tempCode.Replace($"using {@using};", "");
                            }

                        }

                    }


                    if (CS0104SHUT || CS0234SHUT || CSO246SHUT)
                    {

                        ErrorRetryCount += 1;
                        if (ErrorRetryCount < 2)
                        {

                            foreach (var item in tempCache)
                            {

                                option.Add(tempCache[item.Key], SyntaxInfos.TreeUsingMapping[item.Key]);

                            }

                            SyntaxInfos = option;
                            return GetAssembly();

                        }

                    }


                    ComplieException.Diagnostics.AddRange(result.Errors);
                    ComplieException.ErrorFlag = ComplieError.Complie;
                    ComplieException.Message = "发生错误,无法生成程序集！";


                    NError logError = new NError();
                    logError.Handler(result.Compilation, ComplieException.Diagnostics);
                    ComplieException.Log = logError.Buffer.ToString();
                    if (NError.Enabled) { logError.Write(); }

                }
                else
                {

                    NSucceed logSucceed = new NSucceed();
                    logSucceed.Handler(result.Compilation);
                    ComplieException.ErrorFlag = ComplieError.None;
                    ComplieException.Log = logSucceed.Buffer.ToString();
                    if (NSucceed.Enabled) { logSucceed.Write(); }

                }

            }
            return assembly;

        }




        /// <summary>
        /// 获取编译后的类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public Type GetType(string typeName)
        {

            Assembly assembly = GetAssembly();
            if (assembly == null)
            {
                return null;
            }


            var type = assembly.GetTypes().First(item => item.Name == typeName);
            if (type == null)
            {

                ComplieException.ErrorFlag = ComplieError.Type;
                ComplieException.Message = $"发生错误,无法从程序集{assembly.FullName}中获取类型{typeName}！";

            }


            return type;

        }




        /// <summary>
        /// 获取编译后的方法元数据
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public MethodInfo GetMethod(string typeName, string methodName = null)
        {

            var type = GetType(typeName);
            if (type == null)
            {
                return null;
            }


            var info = type.GetMethod(methodName);
            if (info == null)
            {

                ComplieException.ErrorFlag = ComplieError.Method;
                ComplieException.Message = $"发生错误,无法从类型{typeName}中找到{methodName}方法！";


            }


            return info;

        }




        /// <summary>
        /// 获取编译后的委托
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate GetDelegate(string typeName, string methodName, Type delegateType, object binder = null)
        {

            var info = GetMethod(typeName, methodName);
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

                ComplieException.ErrorFlag = ComplieError.Delegate;
                ComplieException.Message = $"发生错误,无法从方法{methodName}创建{delegateType.GetDevelopName()}委托！";

            }


            return null;

        }

        public T GetDelegate<T>(string typeName, string methodName, object binder = null) where T : Delegate
        {

            return (T)GetDelegate(typeName, methodName, typeof(T));

        }


    }

}
