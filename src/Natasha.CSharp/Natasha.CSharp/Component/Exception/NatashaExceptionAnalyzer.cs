using Microsoft.CodeAnalysis;
using Natasha.CSharp.Error.Model;
using System.Linq;

namespace Natasha.CSharp.Core
{
    public class NatashaExceptionAnalyzer
    {

        public static NatashaException? GetSyntaxException(SyntaxTree tree)
        {

            var diagnostics = tree.GetDiagnostics();
            var errors = diagnostics.Where(item => !item.IsSuppressed).ToArray();
            if (errors.Length>0)
            {
                var first = errors[0];
                var exception = new NatashaException(first.GetMessage());
                exception.Diagnostics.AddRange(errors);
                exception.Formatter = tree.ToString();
                exception.ErrorKind = ExceptionKind.Syntax;
                return exception;
            }
            return null;

        }
        /*
        public static List<NatashaException> GetCompileException(string assemblyName, ImmutableArray<Diagnostic> errors)
        {

            var exceptions = new Dictionary<string, NatashaException>();
            var results = new List<NatashaException>();
            foreach (var item in errors)
            {

                var tree = item.Location.SourceTree;
                if (tree == null)
                {

                    if (results.Count == 0)
                    {
                        results.Add(new NatashaException($"编译错误 : {item.Id} {item.GetMessage()}") { ErrorFlag = ExceptionKind.Compile, Name = assemblyName });
                    }
                    results[0].Diagnostics.Add(item);

                }
                else
                {
                    var key = tree.ToString();
                    if (!exceptions.ContainsKey(key))
                    {
                        exceptions[key] = new NatashaException($"编译错误 : {item.Id} {item.GetMessage()}") { ErrorFlag = ExceptionKind.Compile, Name = assemblyName, Formatter = key };
                    }
                    exceptions[key].Diagnostics.Add(item);

                }

            }
            results.AddRange(exceptions.Values);
            return results;

        }
        */
    }
}
