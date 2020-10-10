using Microsoft.CodeAnalysis;
using Natasha.CSharpEngine;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace Natasha.CSharp
{

    public static class AssemblyBuilderExtension
    {

        public static Type GetTypeFromFullName(this AssemblyCSharpBuilder builder, string typeName)
        {

            Assembly assembly = builder.GetAssembly();
            if (assembly == null)
            {

                return null;

            }


            var type = assembly.GetTypes().First(item => item.GetDevelopName() == typeName);
            if (type == null)
            {

                CompilationException exception = new CompilationException($"无法在程序集 {builder.Compiler.AssemblyName} 中找到该类型 {typeName}！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Type;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return type;

        }
        public static Type GetTypeFromShortName(this AssemblyCSharpBuilder builder, string typeName)
        {

            Assembly assembly = builder.GetAssembly();
            if (assembly == null)
            {

                return null;

            }


            var type = assembly.GetTypes().First(item => item.Name == typeName);
            if (type == null)
            {

                CompilationException exception = new CompilationException($"无法在程序集 {builder.Compiler.AssemblyName} 中找到该类型 {typeName}！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Type;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return type;

        }
        
        
        
        
        public static MethodInfo GetMethodFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
        {

            var type = builder.GetTypeFromFullName(typeName);
            if (type == null)
            {
                return null;
            }


            var info = type.GetMethod(methodName);
            if (info == null)
            {

                CompilationException exception = new CompilationException($"无法在类型 {typeName} 中找到该方法 {methodName}！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Method;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return info;

        }
        public static MethodInfo GetMethodFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
        {

            var type = builder.GetTypeFromShortName(typeName);
            if (type == null)
            {
                return null;
            }


            var info = type.GetMethod(methodName);
            if (info == null)
            {

                CompilationException exception = new CompilationException($"无法在类型 {typeName} 中找到该方法 {methodName}！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Method;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return info;

        }
        
        
        
        
        public static Delegate GetDelegateFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object target = null)
        {

            var info = builder.GetMethodFromFullName(typeName, methodName);
            if (info == null)
            {
                return null;
            }


            try
            {

                return info.CreateDelegate(delegateType, target);

            }
            catch (Exception ex)
            {

                CompilationException exception = new CompilationException($"在类型 {typeName} 中找到的方法 {methodName} 向 {delegateType.FullName} 转换时出错！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Delegate;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return null;

        }
        public static T GetDelegateFromFullName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object target = null) where T : Delegate
        {
            return (T)GetDelegateFromFullName(builder,typeName, methodName, typeof(T), target);
        }
        public static Delegate GetDelegateFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object target = null)
        {

            var info = builder.GetMethodFromShortName(typeName, methodName);
            if (info == null)
            {
                return null;
            }


            try
            {

                return info.CreateDelegate(delegateType, target);
  
            }
            catch (Exception ex)
            {

                CompilationException exception = new CompilationException($"在类型 {typeName} 中找到的方法 {methodName} 向 {delegateType.FullName} 转换时出错！");
                if (builder.Exceptions.Count == 0)
                {
                    exception.ErrorFlag = ExceptionKind.Delegate;
                }
                else
                {
                    exception.ErrorFlag = ExceptionKind.Assembly;
                }
                builder.Exceptions.Add(exception);

            }
            return null;

        }
        public static T GetDelegateFromShortName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object target = null) where T : Delegate
        {
            return (T)GetDelegateFromShortName(builder, typeName, methodName, typeof(T), target);
        }




        #region SimpleLinkApi
        public static AssemblyCSharpBuilder AutoUsing(this AssemblyCSharpBuilder builder)
        {
            builder.CustomUsingShut = false;
            return builder;
        }
        public static AssemblyCSharpBuilder CustomUsing(this AssemblyCSharpBuilder builder)
        {
            builder.CustomUsingShut = true;
            return builder;
        }
        public static AssemblyCSharpBuilder ThrowCompilerError(this AssemblyCSharpBuilder builder)
        {
            builder.CompileErrorBehavior = ExceptionBehavior.Throw;
            return builder;
        }
        public static AssemblyCSharpBuilder ThrowAndLogCompilerError(this AssemblyCSharpBuilder builder)
        {
            builder.CompileErrorBehavior = ExceptionBehavior.Log | ExceptionBehavior.Throw;
            return builder;
        }
        public static AssemblyCSharpBuilder LogCompilerError(this AssemblyCSharpBuilder builder)
        {
            builder.CompileErrorBehavior = ExceptionBehavior.Log;
            return builder;
        }
        public static AssemblyCSharpBuilder IgnoreCompilerError(this AssemblyCSharpBuilder builder)
        {
            builder.CompileErrorBehavior = ExceptionBehavior.None;
            return builder;
        }




        public static AssemblyCSharpBuilder ThrowAndLogSyntaxError(this AssemblyCSharpBuilder builder)
        {
            builder.SyntaxErrorBehavior = ExceptionBehavior.Log | ExceptionBehavior.Throw;
            return builder;
        }
        public static AssemblyCSharpBuilder ThrowSyntaxError(this AssemblyCSharpBuilder builder)
        {
            builder.SyntaxErrorBehavior = ExceptionBehavior.Throw;
            return builder;
        }
        public static AssemblyCSharpBuilder ForceAddSyntax(this AssemblyCSharpBuilder builder)
        {
            builder.SyntaxErrorBehavior = ExceptionBehavior.Ignore;
            return builder;
        }
        public static AssemblyCSharpBuilder LogSyntaxError(this AssemblyCSharpBuilder builder)
        {
            builder.SyntaxErrorBehavior = ExceptionBehavior.Log;
            return builder;
        }
        public static AssemblyCSharpBuilder IgnoreSyntaxError(this AssemblyCSharpBuilder builder)
        {
            builder.SyntaxErrorBehavior = ExceptionBehavior.None;
            return builder;
        }
        public static AssemblyCSharpBuilder SetAssemblyName(this AssemblyCSharpBuilder builder,string assemblyName)
        {
            builder.Compiler.AssemblyName = assemblyName;
            return builder;
        }
        public static AssemblyCSharpBuilder SetRetryLimit(this AssemblyCSharpBuilder builder,int maxRetry)
        {
            builder.RetryLimit = maxRetry;
            return builder;
        }
        public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder,string folder)
        {
            builder.OutputFolder = folder;
            return builder;
        }



        public static AssemblyCSharpBuilder UseFileCompile(this AssemblyCSharpBuilder builder, string folder = default)
        {

            if (folder != default)
            {
                builder.OutputFolder = folder;
            }
            builder.Compiler.AssemblyOutputKind = AssemblyBuildKind.File;
            return builder;

        }
        public static AssemblyCSharpBuilder UseStreamCompile(this AssemblyCSharpBuilder builder)
        {
            builder.Compiler.AssemblyOutputKind = AssemblyBuildKind.Stream;
            return builder;
        }
        #endregion

        public static AssemblyCSharpBuilder AddRetryHandler(this AssemblyCSharpBuilder builder, string key, Func<Diagnostic, SyntaxBase, string, string> action)
        {
            NatashaCSharpEngine.ErrorHandlers[key] = action;
            return builder;
        }

    }

}
