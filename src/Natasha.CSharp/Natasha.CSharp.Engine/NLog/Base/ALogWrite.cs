using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Log.Model
{
    public abstract class ALogWrite
    {

        public string FormartCode;
        public StringBuilder Buffer;
        public abstract void  Write();


        public ALogWrite() => Buffer = new StringBuilder();



        /// <summary>
        /// 为代码增加行号
        /// </summary>
        /// <param name="model">代码内容</param>
        /// <returns></returns>
        public StringBuilder AddLineNumber(IEnumerable<SyntaxTree> trees)
        {

            StringBuilder builder = new StringBuilder();
            foreach (var item in trees)
            {
                builder.AppendLine(item.ToString());
            }


            var arrayLines = builder.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i += 1)
            {

                Buffer.AppendLine($"{i + 1}\t{arrayLines[i]}");

            }
            return builder;

        }




        public StringBuilder AddLineNumber(string code)
        {

            StringBuilder builder = new StringBuilder();
            var arrayLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i += 1)
            {

                Buffer.AppendLine($"{i + 1}\t{arrayLines[i]}");

            }
            return builder;

        }

    }

}
