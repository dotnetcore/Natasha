using Microsoft.CodeAnalysis;
using Natasha.CSharpEngine.Error;
using Natasha.CSharpEngine.Log;
using Natasha.CSharpSyntax;
using Natasha.Error;
using Natasha.Error.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Natasha.CSharpEngine.Syntax
{

    public class NatashaCSharpSyntax : CSharpSyntaxBase
    {

        public readonly ConcurrentDictionary<string, HashSet<string>> UsingCache;
        public ExceptionBehavior ErrorBehavior;
        public NatashaCSharpSyntax()
        {
            ErrorBehavior = ExceptionBehavior.Throw;
            UsingCache = new ConcurrentDictionary<string, HashSet<string>>();
        }




        public CompilationException Add(string script, HashSet<string> sets = default)
        {

            var tree = AddTreeToCache(script);
            var exception = NatashaException.GetSyntaxException(tree);
            if (!exception.HasError || ErrorBehavior == ExceptionBehavior.Ignore)
            {

                UsingCache[exception.Formatter] = sets;

            }
            else
            {

                HandlerErrors(exception);

            }
            return exception;

        }




        public CompilationException Add(SyntaxTree node, HashSet<string> sets = default)
        {

            return Add(node.ToString(), sets);

        }




        public CompilationException Add(IScript node)
        {

            return Add(node.Script, node.Usings);

        }




        public CompilationException AddFile(string filePath)
        {

            return Add(File.ReadAllText(filePath));

        }




        private void HandlerErrors(CompilationException exception)
        {

            if (ErrorBehavior == ExceptionBehavior.Throw)
            {

                throw exception;

            }
            else if (ErrorBehavior == ExceptionBehavior.Log)
            {

                LogOperator.ErrorRecoder(exception);

            }
            else if (ErrorBehavior == (ExceptionBehavior.Log | ExceptionBehavior.Throw))
            {

                LogOperator.ErrorRecoder(exception);
                throw exception;

            }

        }



        public CompilationException Update(string old, string @new, HashSet<string> sets = default)
        {

            if (TreeCache.ContainsKey(old))
            {

                while (!TreeCache.TryRemove(old,out _)) { };

            }
            if (sets == default)
            {

                if (UsingCache.ContainsKey(old))
                {
                    sets = UsingCache[old];
                    while (!UsingCache.TryRemove(old, out _)) { };
                }
                
            }
            return Add(@new,sets);

        }
    }
}
