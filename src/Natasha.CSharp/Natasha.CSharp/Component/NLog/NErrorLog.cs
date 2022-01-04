using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Component.Log;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System
{

    public class NErrorLog : NatashaLogBase
    {

        internal protected override async Task Write()
        {
            await NatashaWriterIniter<NErrorLog>.Recoder(_logBuffer);
        }

        public void Handler(CSharpCompilation compilation, IEnumerable<Diagnostic> diagnostics)
        {
            Dictionary<SyntaxTree, List<Diagnostic>> syntaxCache = new();
            List<string> others = new List<string>();
            foreach (var item in diagnostics)
            {
              
                if (item.Location == Location.None)
                {

                    others.Add(item.GetMessage());

                }
                else if(item.Location.SourceTree != null)
                {
                   
                    var tree = item.Location.SourceTree;
                    if (!syntaxCache.ContainsKey(tree))
                    {
                        syntaxCache[tree] = new List<Diagnostic>();
                    }
                    syntaxCache[tree].Add(item);

                }

            }

            _logBuffer.AppendLine($"\r\n\r\n========================Error : {compilation.AssemblyName}========================\r\n");
            foreach (var item in syntaxCache)
            {
                var codeText = item.Key.ToString();
                _logBuffer.AppendLine();
                _logBuffer.Append(AddLineNumber(codeText));
                _logBuffer.AppendLine("\r\n\r\n-----------------------------------------------error---------------------------------------------------");
                _logBuffer.AppendLine($"\r\n    Time     :\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                _logBuffer.AppendLine($"\r\n    Lauguage :\t{compilation.Language} & {compilation.LanguageVersion}");
                _logBuffer.AppendLine($"\r\n    Target   :\t{compilation.AssemblyName}");
                _logBuffer.AppendLine($"\r\n    Error    :\t共{item.Value.Count}处错误！");


                foreach (var error in item.Value)
                {
                    var span = error.Location.GetLineSpan();
                    var result = GetErrorString(codeText, error.Location.GetLineSpan());
                    _logBuffer.AppendLine($"\t\t第{span.StartLinePosition.Line + 1}行，第{span.StartLinePosition.Character}个字符：       内容【{result}】  {error.GetMessage()}");
                }

                if (others.Count>0)
                {
                    _logBuffer.AppendLine("\r\n-----------------------------------------------其他信息-----------------------------------------------");
                    foreach (var message in others)
                    {
                        _logBuffer.AppendLine($"\r\n    Message :\t{message}");
                    }
                }

                _logBuffer.AppendLine("\r\n====================================================================\r\n");

            }

        }

        public void Handler(List<Diagnostic> diagnostics)
        {

            string formartCode = diagnostics[0].Location.SourceTree!.ToString();
            _logBuffer.AppendLine($"\r\n\r\n========================Error : 语法错误 ========================\r\n");
            _logBuffer.AppendLine();
            _logBuffer.Append(AddLineNumber(formartCode));
            _logBuffer.AppendLine("\r\n\r\n-----------------------------------------------error---------------------------------------------------");
            _logBuffer.AppendLine($"\r\n    Time :\t\t{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            _logBuffer.AppendLine($"\r\n    Error:\t\t共{diagnostics.Count}处错误！");


            foreach (var error in diagnostics)
            {

                var temp = error.Location.GetLineSpan().StartLinePosition;
                var result = GetErrorString(formartCode, error.Location.GetLineSpan());
                _logBuffer.AppendLine($"\t\t第{temp.Line + 1}行，第{temp.Character}个字符：       内容【{result}】  {error.GetMessage()}");

            }
            _logBuffer.AppendLine("\r\n====================================================================\r\n");

        }

        private static string GetErrorString(string content, FileLinePositionSpan linePositionSpan)
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
                builder.AppendLine(currentErrorLine.Substring(start.Character));
                for (int i = start.Line; i < end.Line - 1; i += 1)
                {

                    builder.AppendLine("\t\t\t" + arrayLines[i]);

                }
                currentErrorLine = arrayLines[end.Line];
                builder.AppendLine(currentErrorLine.Substring(0, end.Character));
                return builder.ToString();

            }

        }

    }

}
