using Microsoft.CodeAnalysis;
using Natasha.Error.Model;
using System;
using System.Collections.Generic;

namespace Natasha.Error
{
    [Serializable]
    public class CompilationException : Exception
    {

        public CompilationException(string message) : base(message)
        {
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }
        public CompilationException(string message, Exception inner) : base(message, inner)
        {
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }
        protected CompilationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;
        }


        public CompilationException()
        {

            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ExceptionKind.None;

        }


        public bool HasError
        {
            get { return Diagnostics.Count > 0; }
        }


        //名称标识
        public string Name;

        //编译日志
        public string Log;

        //格式化后的脚本字符串
        public string Formatter;

        //错误类型
        public ExceptionKind ErrorFlag;

        //出错的语法树
        public SyntaxTree Tree;

        //roslyn诊断集合
        public List<Diagnostic> Diagnostics;

    }


}
