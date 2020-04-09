using Natasha.Error;
using Natasha.Error.Model;
using System;
using System.Linq;
using System.Reflection;

namespace Natasha
{

    public static class AssemblyBuilderExtension
    {

        public static Type GetTypeFromFullName(this AssemblyBuilder builder, string typeName)
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
        public static Type GetTypeFromShortName(this AssemblyBuilder builder, string typeName)
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
        
        
        
        
        public static MethodInfo GetMethodFromFullName(this AssemblyBuilder builder, string typeName, string methodName)
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
        public static MethodInfo GetMethodFromShortName(this AssemblyBuilder builder, string typeName, string methodName)
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
        
        
        
        
        public static Delegate GetDelegateFromFullName(this AssemblyBuilder builder, string typeName, string methodName, Type delegateType, object binder = null)
        {

            var info = builder.GetMethodFromFullName(typeName, methodName);
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
        public static T GetDelegateFromFullName<T>(this AssemblyBuilder builder, string typeName, string methodName, object binder = null) where T : Delegate
        {
            return (T)(builder.GetDelegateFromFullName(typeName, methodName, typeof(T)));
        }
        public static Delegate GetDelegateFromShortName(this AssemblyBuilder builder, string typeName, string methodName, Type delegateType, object binder = null)
        {

            var info = builder.GetMethodFromShortName(typeName, methodName);
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
        public static T GetDelegateFromShortName<T>(this AssemblyBuilder builder, string typeName, string methodName, object binder = null) where T : Delegate
        {
            return (T)(builder.GetDelegateFromShortName(typeName, methodName, typeof(T)));
        }

    }

}
