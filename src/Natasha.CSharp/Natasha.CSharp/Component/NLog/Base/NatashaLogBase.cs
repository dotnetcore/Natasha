using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.CSharp.Component.Log
{
    public abstract class NatashaLogBase
    {

        //private string? _formartCode;
        internal protected readonly StringBuilder _logBuffer;
        internal protected abstract Task Write();


        public NatashaLogBase() => _logBuffer = new StringBuilder();



        /// <summary>
        /// 为代码增加行号
        /// </summary>
        /// <param name="model">代码内容</param>
        /// <returns></returns>
        internal static StringBuilder AddLineNumber(SyntaxTree tree)
        {
            return AddLineNumber(tree.ToString());

        }




        internal static StringBuilder AddLineNumber(string code)
        {

            StringBuilder builder = new StringBuilder();
            var arrayLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i += 1)
            {

                builder.AppendLine($"{i + 1}\t{arrayLines[i]}");

            }
            return builder;

        }

    }

}
