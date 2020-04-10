using Natasha.CSharpEngine;
using Natasha.CSharpEngine.Compile;
using Natasha.CSharpEngine.Log;
using Natasha.CSharpEngine.Syntax;
using Natasha.Error.Model;
using System;
using System.IO;
using System.Reflection;

namespace Natasha.CSharp
{

    public class AssemblyCSharpBuilder : NatashaEngine
    {

        /// <summary>
        /// 默认的输出文件夹
        /// </summary>
        public static string OutputFolder;
        static AssemblyCSharpBuilder()
        {
            OutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DynamicLibraryFolders");
        }



        public bool CustomerUsingShut;
        public AssemblyCSharpBuilder() : this(Guid.NewGuid().ToString("N")){}
        public AssemblyCSharpBuilder(string name):base(name)
        {

            CustomerUsingShut = false;
            RetryLimit = 2;

        }




        public void CompilerOption(Action<NatashaCompiler> action)
        {
            action?.Invoke(Compiler);
        }
        public void SyntaxOptions(Action<NatashaSyntax> action)
        {
            action?.Invoke(Syntax);
        }




        public Assembly GetAssembly()
        {

            //如果是文件编译，则初始化路径
            if (Compiler.AssemblyOutputKind == Framework.AssemblyBuildKind.File)
            {

                if (Compiler.DllPath == default)
                {
                    Compiler.DllPath = Path.Combine(OutputFolder, Compiler.AssemblyName, ".dll");
                    Compiler.PdbPath = Path.Combine(OutputFolder, Compiler.AssemblyName, ".pdb");
                }

            }


            //进入编译流程
            Compile();


            //如果编译出错
            if (Compiler.Assembly == null && Exceptions != null && Exceptions.Count > 0 )
            {

                switch (Compiler.ErrorBehavior)
                {

                    case ExceptionBehavior.Log | ExceptionBehavior.Throw:
                        LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                        throw Exceptions[0];
                    case ExceptionBehavior.Log:
                        LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                        break;
                    case ExceptionBehavior.Throw:
                        throw Exceptions[0];
                    default:
                        break;

                }

            }
            else
            {

                LogOperator.SucceedRecoder(Compiler.Compilation);

            }
            return Compiler.Assembly;

        }



        ///// <summary>
        ///// 获取编译后的类型
        ///// </summary>
        ///// <param name="typeName">类型名称</param>
        ///// <returns></returns>
        //public Type GetType(string typeName)
        //{

        //    Assembly assembly = GetAssembly();
        //    if (assembly == null)
        //    {

        //        return null;

        //    }


        //    var type = assembly.GetTypes().First(item => item.Name == typeName);
        //    if (type == null)
        //    {

        //        CompilationException exception = new CompilationException($"无法在程序集 {AssemblyName} 中找到该类型 {typeName}！");
        //        if (Exceptions.Count == 0)
        //        {
        //            exception.ErrorFlag = ExceptionKind.Type;
        //        }
        //        else
        //        {
        //            exception.ErrorFlag = ExceptionKind.Assembly;
        //        }
        //        Exceptions.Add(exception);

        //    }
        //    return type;

        //}




        ///// <summary>
        ///// 获取编译后的方法元数据
        ///// </summary>
        ///// <param name="typeName">类型名称</param>
        ///// <param name="methodName">方法名</param>
        ///// <returns></returns>
        //public MethodInfo GetMethod(string typeName, string methodName = null)
        //{

        //    var type = GetType(typeName);
        //    if (type == null)
        //    {
        //        return null;
        //    }


        //    var info = type.GetMethod(methodName);
        //    if (info == null)
        //    {

        //        CompilationException exception = new CompilationException($"无法在类型 {typeName} 中找到该方法 {methodName}！");
        //        if (Exceptions.Count == 0)
        //        {
        //            exception.ErrorFlag = ExceptionKind.Method;
        //        }
        //        else
        //        {
        //            exception.ErrorFlag = ExceptionKind.Assembly;
        //        }
        //        Exceptions.Add(exception);

        //    }
        //    return info;

        //}




        ///// <summary>
        ///// 获取编译后的委托
        ///// </summary>
        ///// <param name="typeName">类型名称</param>
        ///// <param name="methodName">方法名</param>
        ///// <param name="delegateType">委托类型</param>
        ///// <returns></returns>
        //public Delegate GetDelegate(string typeName, string methodName, Type delegateType, object binder = null)
        //{

        //    var info = GetMethod(typeName, methodName);
        //    if (info == null)
        //    {
        //        return null;
        //    }


        //    try
        //    {


        //        return info.CreateDelegate(delegateType, binder);

        //    }
        //    catch (Exception ex)
        //    {

        //        CompilationException exception = new CompilationException($"在类型 {typeName} 中找到的方法 {methodName} 向 {delegateType.FullName} 转换时出错！");
        //        if (Exceptions.Count == 0)
        //        {
        //            exception.ErrorFlag = ExceptionKind.Delegate;
        //        }
        //        else
        //        {
        //            exception.ErrorFlag = ExceptionKind.Assembly;
        //        }
        //        Exceptions.Add(exception);

        //    }
        //    return null;

        //}

        //public T GetDelegate<T>(string typeName, string methodName, object binder = null) where T : Delegate
        //{

        //    return (T)GetDelegate(typeName, methodName, typeof(T));

        //}





    }
}
