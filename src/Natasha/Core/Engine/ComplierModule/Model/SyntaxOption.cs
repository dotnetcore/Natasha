using Microsoft.CodeAnalysis;
using Natasha.Log;
using System.Collections.Generic;
namespace Natasha.Complier.Model
{
    public class SyntaxOption
    {

        public readonly List<SyntaxTree> Trees;
        public readonly List<CompilationException> SyntaxExceptions;


        public SyntaxOption()
        {

            Trees = new List<SyntaxTree>();
            SyntaxExceptions = new List<CompilationException>();

        }




        public CompilationException Add(string content)
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

                Trees.Add(tree);

            }
            SyntaxExceptions.Add(exception);
            return exception;


        }


    }
}
