#if NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Natasha.CSharp.Extension.Inner
{

    internal static class CSharpCompilationExtension
    {
        internal static NatashaCompilationLog GetNatashaLog(this CSharpCompilation compilation)
        {
            NatashaCompilationLog natashaCompilation = new();
            natashaCompilation.AddCompilationInfo("AssemblyName", compilation.AssemblyName ?? string.Empty);
            natashaCompilation.AddCompilationInfo("Language", compilation.Language);
            natashaCompilation.AddCompilationInfo("LanguageVersion", compilation.LanguageVersion.ToString());
            natashaCompilation.AddCompilationInfo("SyntaxTreeCount", compilation.SyntaxTrees.Length.ToString());
            natashaCompilation.AddCompilationInfo("ReferencesCount", compilation.References.Count().ToString());
            var errors = compilation.GetDiagnostics();
            if (errors.Length > 0)
            {
                Dictionary<SyntaxTree, List<Diagnostic>> syntaxCache = new();
                foreach (var item in compilation.GetDiagnostics())
                {
                    if (item.Location.SourceTree != null)
                    {
                        var tree = item.Location.SourceTree;
                        if (!syntaxCache.ContainsKey(tree))
                        {
                            syntaxCache[tree] = new List<Diagnostic>();
                        }
                        syntaxCache[tree].Add(item);
                    }
                }
                natashaCompilation.HasError = true;
                foreach (var item in syntaxCache)
                {
                    var codeText = item.Key.ToString();
                    StringBuilder errorMessage = new();
                    foreach (var error in item.Value)
                    {
                        var span = error.Location.GetLineSpan();
                        errorMessage.AppendLine($"第{span.StartLinePosition.Line + 1}行，第{span.StartLinePosition.Character}个字符： 内容【{GetErrorMessage(codeText, error.Location.GetLineSpan())}】  {error.GetMessage()}");
                    }
                    natashaCompilation.AddMessage(item.Value.Count, AddLineNumber(codeText), errorMessage.ToString());
                }
            }
            else
            {
                natashaCompilation.HasError = false;
                foreach (var item in compilation.SyntaxTrees)
                {
                    natashaCompilation.AddMessage(0, AddLineNumber(item.ToString()), item.GetFirstOopName());
                }
            }
            return natashaCompilation;
        }
        private static string GetErrorMessage(string content, FileLinePositionSpan linePositionSpan)
        {

            var start = linePositionSpan.StartLinePosition;
            var end = linePositionSpan.EndLinePosition;


            var arrayLines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var currentErrorLine = arrayLines[start.Line];


            if (start.Line == end.Line)
            {

                if (start.Character == end.Character)
                {

                    return currentErrorLine.Trim();

                }
                else
                {

                    return currentErrorLine[start.Character..end.Character].Trim();

                }

            }
            else
            {

                StringBuilder builder = new();
                builder.AppendLine(currentErrorLine[start.Character..]);
                for (int i = start.Line; i < end.Line - 1; i += 1)
                {

                    builder.AppendLine(arrayLines[i]);

                }
                currentErrorLine = arrayLines[end.Line];
                builder.AppendLine(currentErrorLine[..end.Character]);
                return builder.ToString();

            }

        }
        private static string AddLineNumber(string code)
        {

            StringBuilder builder = new();
            var arrayLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < arrayLines.Length; i += 1)
            {

                builder.AppendLine($"{i + 1}\t{arrayLines[i]}");

            }
            return builder.ToString();

        }
    }

}
#endif


