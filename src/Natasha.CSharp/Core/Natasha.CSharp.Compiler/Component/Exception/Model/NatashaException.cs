using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;



[Serializable]
public sealed class NatashaException : Exception
{

    public NatashaException(string message) : base(message)
    {
        Formatter = string.Empty;
        Diagnostics = new List<Diagnostic>();
        ErrorKind = NatashaExceptionKind.None;
        CompileMessage = string.Empty;
    }

    //格式化后的脚本字符串
    public string Formatter;

    //错误类型
    public NatashaExceptionKind ErrorKind;

    //roslyn诊断集合
    public List<Diagnostic> Diagnostics;

    /// <summary>
    /// 详细的编译信息
    /// </summary>
    public string CompileMessage;

}



