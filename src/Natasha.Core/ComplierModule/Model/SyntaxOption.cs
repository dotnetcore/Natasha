using Microsoft.CodeAnalysis;
using Natasha.Log;
using System.Collections.Generic;
namespace Natasha.Core.Complier.Model
{
    public class SyntaxOption
    {

        public readonly List<CompilationException> SyntaxExceptions;
        public readonly Dictionary<SyntaxTree, HashSet<string>> TreeUsingMapping;
        public readonly Dictionary<SyntaxTree, string> TreeCodeMapping;
        public SyntaxOption()
        {

            SyntaxExceptions = new List<CompilationException>();
            TreeUsingMapping = new Dictionary<SyntaxTree, HashSet<string>>();
            TreeCodeMapping = new Dictionary<SyntaxTree, string>();

        }




        public CompilationException Add(string content, HashSet<string> sets = default)
        {

            CompilationException exception = new CompilationException();
            var (tree, formartter, errors) = content;


            exception.Formatter = formartter;
            exception.Diagnostics.AddRange(errors);


            if (exception.Diagnostics.Count != 0)
            {

                exception.ErrorFlag = ComplieError.Syntax;
                exception.Message = "语法错误,请仔细检查脚本代码！";
                NError log = new NError();
                log.Handler(exception.Diagnostics);
                log.Write();
                exception.Log = log.Buffer.ToString();

            }
            else
            {

                TreeCodeMapping.Add(tree, content);
                TreeUsingMapping[tree] = sets;

            }
            SyntaxExceptions.Add(exception);
            return exception;


        }


    }
}
