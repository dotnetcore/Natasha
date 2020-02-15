using Microsoft.CodeAnalysis;
using Natasha.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
namespace Natasha.Core.Complier.Model
{
    public class SyntaxOption
    {

        public readonly List<CompilationException> SyntaxExceptions;
        public readonly ConcurrentDictionary<SyntaxTree, HashSet<string>> TreeUsingMapping;
        public readonly ConcurrentDictionary<SyntaxTree, string> TreeCodeMapping;
        public SyntaxOption()
        {

            SyntaxExceptions = new List<CompilationException>();
            TreeUsingMapping = new ConcurrentDictionary<SyntaxTree, HashSet<string>>();
            TreeCodeMapping = new ConcurrentDictionary<SyntaxTree, string>();

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
                NErrorLog log = new NErrorLog();
                log.Handler(exception.Diagnostics);
                if (NErrorLog.Enabled) { log.Write(); }
                exception.Log = log.Buffer.ToString();

            }
            else
            {

                TreeCodeMapping[tree] = content;
                TreeUsingMapping[tree] = sets;

            }

            SyntaxExceptions.Add(exception);
            return exception;


        }

        public CompilationException Add(SyntaxTree node)
        {

            CompilationException exception = new CompilationException();
            var (tree, formartter, errors) = node;


            exception.Formatter = formartter;
            exception.Diagnostics.AddRange(errors);


            if (exception.Diagnostics.Count != 0)
            {

                exception.ErrorFlag = ComplieError.Syntax;
                exception.Message = "语法错误,请仔细检查脚本代码！";
                NErrorLog log = new NErrorLog();
                log.Handler(exception.Diagnostics);
                if (NErrorLog.Enabled) { log.Write(); }
                exception.Log = log.Buffer.ToString();

            }
            else
            {


                TreeCodeMapping[tree] = tree.ToString();
                TreeUsingMapping[tree] = default;

            }
            SyntaxExceptions.Add(exception);
            return exception;

        }

    }

}
