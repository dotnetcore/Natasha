using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Natasha
{
    public class CompilationException
    {

        public CompilationException()
        {

            Diagnostics = new List<Diagnostic>();
            ErrorFlag = ComplieError.None;

        }


        //脚本字符串
        public string Source;

        //错误信息
        public string Message;

        //格式化后的脚本字符串
        public string Formatter;

        //错误类型
        public ComplieError ErrorFlag;

        //roslyn诊断集合
        public List<Diagnostic> Diagnostics;
       
    }



    public enum ComplieError
    {
        None,
        Assembly,
        Type,
        Method,
        Delegate
    }

}
