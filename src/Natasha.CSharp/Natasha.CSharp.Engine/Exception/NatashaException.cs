using Microsoft.CodeAnalysis;
using Natasha.Error;
using Natasha.Error.Model;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Natasha.CSharpEngine.Error
{
    public class NatashaException
    {

        public static CompilationException GetSyntaxException(SyntaxTree tree)
        {

            CompilationException exception;
            var diagnostics = tree.GetDiagnostics();
            if (diagnostics==null)
            {

                exception = new CompilationException();
            }
            else
            {

                StringBuilder builder = new StringBuilder();
                foreach (var item in diagnostics)
                {
                    builder.AppendLine(item.GetMessage());
                }
                exception = new CompilationException(builder.ToString());
                exception.Formatter = tree.ToString();
                builder.Insert(0, exception.Formatter);
                exception.Log = builder.ToString();
                exception.Diagnostics.AddRange(diagnostics);
                exception.ErrorFlag = ExceptionKind.Syntax;

            }
            exception.Tree = tree;
            return exception;

        }
        public static List<CompilationException> GetCompileException(string assemblyName, ImmutableArray<Diagnostic> errors)
        {

            var exceptions = new Dictionary<string, CompilationException>();
            var results = new List<CompilationException>();
            foreach (var item in errors)
            {

                var tree = item.Location.SourceTree;
                if (tree == null)
                {

                    if (results.Count == 0)
                    {
                        results.Add(new CompilationException($"编译错误 : {item.Id} {item.GetMessage()}") { ErrorFlag = ExceptionKind.Compile, Name = assemblyName });
                    }
                    results[0].Diagnostics.Add(item);

                }
                else
                {
                    var key = tree.ToString();
                    if (!exceptions.ContainsKey(key))
                    {
                        exceptions[key] = new CompilationException($"编译错误 : {item.Id} {item.GetMessage()}") { ErrorFlag = ExceptionKind.Compile, Name = assemblyName, Formatter = key };
                    }
                    exceptions[key].Diagnostics.Add(item);

                }

            }
            results.AddRange(exceptions.Values);
            return results;

        }
    }
}
