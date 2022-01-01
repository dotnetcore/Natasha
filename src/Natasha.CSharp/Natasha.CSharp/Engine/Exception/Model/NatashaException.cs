using Microsoft.CodeAnalysis;
using Natasha.Error;
using Natasha.Error.Model;
using System;
using System.Collections.Generic;


[Serializable]
public class CompileWrapperExceptions : Exception
{

    public readonly List<NatashaException> CompilerExcpetions;
    public ExceptionKind ErrorFlag;
    public CompileWrapperExceptions(string message) : base(message)
    {
        CompilerExcpetions = new List<NatashaException>();
    }

    public void AddException(NatashaException natashaException)
    {
        CompilerExcpetions.Add(natashaException);
    }

    public bool HasException { get { return CompilerExcpetions.Count > 0; } }

}


namespace Natasha.Error
{

    [Serializable]
    public class NatashaException : Exception
    {

        public NatashaException(string message) : base(message)
        {
            Formatter = string.Empty;
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }
        public NatashaException(string message, Exception inner) : base(message, inner)
        {
            Formatter = string.Empty;
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }
        protected NatashaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            Formatter = string.Empty;
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }


        public NatashaException()
        {
            Formatter = string.Empty;
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;

        }


        public bool HasError
        {
            get { return Diagnostics.Count > 0; }
        }


        //名称标识
        public string? Name;

        //编译日志
        public string? Log;

        //格式化后的脚本字符串
        public string Formatter;

        //错误类型
        public ExceptionKind ErrorFlag;

        //出错的语法树
        public SyntaxTree? Tree;

        //roslyn诊断集合
        public List<Diagnostic> Diagnostics;

    }


}
