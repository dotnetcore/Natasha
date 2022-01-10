using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Reflection;



public static class AssemblyBuilderExtension
{

    public static Type GetTypeFromFullName(this AssemblyCSharpBuilder builder, string typeName)
    {

        Assembly assembly = builder.GetAssembly();
        try
        {
            return assembly.GetTypes().First(item => item.GetDevelopName() == typeName);
        }
        catch (Exception ex)
        {
            NatashaException exception = new($"无法在程序集 {builder.AssemblyName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Type;
            throw exception;
        }

    }
    

    public static MethodInfo GetMethodFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
    {

        var type = GetTypeFromFullName(builder, typeName);
        try
        {
            var info = type.GetMethod(methodName);
            if (info == null)
            {
                throw new Exception("获取方法返回空!");
            }
            return info!;
        }
        catch (Exception ex)
        {

            NatashaException exception = new($"无法在程序集 {builder.AssemblyName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Method;
            throw exception;

        }

    }



    public static Delegate GetDelegateFromFullName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromFullName(builder, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"无法将程序集 {builder.AssemblyName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Delegate;
            throw exception;

        }

    }
    public static T GetDelegateFromFullName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromFullName(builder, typeName, methodName, typeof(T), target);
    }

    

    public static AssemblyCSharpBuilder EnableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.ConfigCompilerOption(opt => opt.SetNullableCompile(NullableContextOptions.Enable));
        return builder;
    }
    public static AssemblyCSharpBuilder DisableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.ConfigCompilerOption(opt => opt.SetNullableCompile(NullableContextOptions.Disable));
        return builder;
    }

    public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder, string folder)
    {
        builder.OutputFolder = folder;
        return builder;
    }
    public static AssemblyCSharpBuilder SetDllFilePath(this AssemblyCSharpBuilder builder, string dllFilePath)
    {
        builder.DllFilePath = dllFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetPdbFilePath(this AssemblyCSharpBuilder builder, string pdbFilePath)
    {
        builder.PdbFilePath = pdbFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetXmlFilePath(this AssemblyCSharpBuilder builder, string xmlFilePath)
    {
        builder.XmlFilePath = xmlFilePath;
        return builder;
    }

    public static AssemblyCSharpBuilder DisableSemanticCheck(this AssemblyCSharpBuilder builder)
    {
        builder.EnableSemanticHandler = false;
        return builder;
    }
    public static AssemblyCSharpBuilder EnableSemanticCheck(this AssemblyCSharpBuilder builder)
    {
        builder.EnableSemanticHandler = true;
        return builder;
    }
}