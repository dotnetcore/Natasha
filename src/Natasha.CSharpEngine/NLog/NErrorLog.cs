using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.Log;
using Natasha.Log.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{

    public class NErrorLog : ALogWrite
    {

        public static bool Enabled;
        static NErrorLog() => Enabled = true;


        public Dictionary<string, List<Diagnostic>> Errors;
        public NErrorLog()
        {
            Errors = new Dictionary<string, List<Diagnostic>>();
        }




        public override void Write()
        {
            NWriter<NErrorLog>.Recoder(Buffer);
        }




        public void Handler(CSharpCompilation compilation, List<Diagnostic> diagnostics)
        {

            Buffer.AppendLine($"\r\n\r\n========================Error : {compilation.AssemblyName}========================\r\n");
            List<string> others = new List<string>();
            foreach (var item in diagnostics)
            {
              
                if (item.Location == Location.None)
                {

                    others.Add(item.ToString());

                }
                else
                {

                    string str = item.Location.SourceTree.ToString();
                    if (!Errors.ContainsKey(str))
                    {
                        Errors[str] = new List<Diagnostic>();
                    }
                    Errors[str].Add(item);

                }

            }

           
            foreach (var item in Errors)
            {

                Buffer.AppendLine();
                Buffer.Append(AddLineNumber(item.Key));
                Buffer.AppendLine("\r\n\r\n-----------------------------------------------error---------------------------------------------------");
                Buffer.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                Buffer.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                Buffer.AppendLine($"\r\n    Target:\t\t{compilation.AssemblyName}");
                Buffer.AppendLine($"\r\n    Error:\t\t共{item.Value.Count}处错误！");


                foreach (var error in item.Value)
                {

                    var temp = error.Location.GetLineSpan().StartLinePosition;
                    var result = GetErrorString(item.Key, error.Location.GetLineSpan());
                    Buffer.AppendLine($"\t\t第{temp.Line + 1}行，第{temp.Character}个字符：       内容【{result}】  {error.GetMessage()}");

                }

                if (others.Count>0)
                {
                    Buffer.AppendLine("\r\n-----------------------------------------------其他信息-----------------------------------------------");
                    foreach (var message in others)
                    {
                        Buffer.AppendLine($"\r\n    Message :\t{message}");
                    }
                }


                Buffer.AppendLine("\r\n====================================================================\r\n");

            }

        }




        public void Handler(List<Diagnostic> diagnostics)
        {

            string formartCode = diagnostics[0].Location.SourceTree.ToString();
            Buffer.AppendLine($"\r\n\r\n========================Error : 语法错误 ========================\r\n");
            Buffer.AppendLine();
            Buffer.Append(AddLineNumber(formartCode));
            Buffer.AppendLine("\r\n\r\n-----------------------------------------------error---------------------------------------------------");
            Buffer.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            Buffer.AppendLine($"\r\n    Error:\t\t共{diagnostics.Count}处错误！");


            foreach (var error in diagnostics)
            {

                var temp = error.Location.GetLineSpan().StartLinePosition;
                var result = GetErrorString(formartCode, error.Location.GetLineSpan());
                Buffer.AppendLine($"\t\t第{temp.Line + 1}行，第{temp.Character}个字符：       内容【{result}】  {error.GetMessage()}");

            }
            Buffer.AppendLine("\r\n====================================================================\r\n");

        }




        internal string GetErrorString(string content, FileLinePositionSpan linePositionSpan)
        {

            var start = linePositionSpan.StartLinePosition;
            var end = linePositionSpan.EndLinePosition;


            var arrayLines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var currentErrorLine = arrayLines[start.Line];


            if (start.Line == end.Line)
            {

                if (start.Character == end.Character)
                {

                    return currentErrorLine.Substring(0, currentErrorLine.Length).Trim();

                }
                else
                {

                    return currentErrorLine.Substring(startIndex: start.Character, end.Character - start.Character).Trim();

                }


            }
            else
            {

                StringBuilder builder = new StringBuilder();
                currentErrorLine.Substring(start.Character);
                builder.AppendLine(currentErrorLine);
                for (int i = start.Line; i < end.Line - 1; i += 1)
                {

                    builder.AppendLine("\t\t\t" + arrayLines[i]);

                }


                currentErrorLine = arrayLines[end.Line];
                currentErrorLine = currentErrorLine.Substring(0, end.Character);
                builder.AppendLine(currentErrorLine);


                return builder.ToString();

            }

        }

    }

}
