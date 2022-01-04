using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class NatashaLogCreator
{
    public static string GetCompilationInfo(this CSharpCompilation compilation)
    {
        return $"AssemblyName:{compilation.AssemblyName};Time:{DateTime.Now:yyyy-MM-dd HH:mm:ss};Lauguage:{compilation.Language};LauguageVersion:{compilation.LanguageVersion};ReferencesCount:{compilation.References.Count()}";
    }
    public static NatashaCompilationLogs GetErrorLog(this CSharpCompilation compilation)
    {

        NatashaCompilationLogs natashaCompilation = new(compilation.GetCompilationInfo());
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
                errorMessage.AppendLine($"\t\t第{span.StartLinePosition.Line + 1}行，第{span.StartLinePosition.Character}个字符：       内容【{GetErrorMessage(codeText, error.Location.GetLineSpan())}】  {error.GetMessage()}");
            }
            natashaCompilation.AddMessage(item.Value.Count, AddLineNumber(codeText), errorMessage.ToString());
        }
        return natashaCompilation;
    }
    public static NatashaCompilationLogs GetSucceedLog(this CSharpCompilation compilation)
    {
        NatashaCompilationLogs natashaCompilation = new(compilation.GetCompilationInfo());
        natashaCompilation.HasError = false;
        foreach (var item in compilation.SyntaxTrees)
        {
            natashaCompilation.AddMessage(0, AddLineNumber(item.ToString()) ,item.GetFirstOopName());
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

                return currentErrorLine[..].Trim();

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

                builder.AppendLine("\t\t\t" + arrayLines[i]);

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

