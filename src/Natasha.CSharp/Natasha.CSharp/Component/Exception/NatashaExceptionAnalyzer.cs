using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Error.Model;
using System.Collections.Immutable;
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
       
        public static NatashaException GetCompileException(CSharpCompilation compilation, ImmutableArray<Diagnostic> errors)
        {
            var first = errors[0];
            var exception = new NatashaException(first.GetMessage());
            exception.Diagnostics.AddRange(errors);
            if (first.Location.SourceTree!=null)
            {
                exception.Formatter = first.Location.SourceTree.ToString();
            }
            exception.CompileMessage = $"编译程序集为:{compilation.AssemblyName};CSharp版本:{compilation.LanguageVersion};";
            exception.ErrorKind = ExceptionKind.Compile;
            return exception;
        }
    }
}
