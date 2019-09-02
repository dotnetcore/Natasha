using Microsoft.CodeAnalysis;
using Natasha.Log;
using System.Collections.Generic;
using System.Runtime.Loader;
namespace Natasha.Complier.Model
{
    public class ComplierModel
    {

        public string AssemblyName;
        public List<SyntaxTree> Trees;
        private AssemblyDomain _domain;
        public List<PortableExecutableReference> References;
        public List<CompilationException> Exceptions;


        public ComplierModel()
        {
            Trees = new List<SyntaxTree>();
            References = new List<PortableExecutableReference>();
            Exceptions = new List<CompilationException>();
            _domain = DomainManagment.Default;
            References.AddRange(_domain.ReferencesCache);
        }




        public AssemblyDomain Domain
        {
            get
            {

#if !NETSTANDARD2_0
                if (AssemblyLoadContext.CurrentContextualReflectionContext != default) 
                {

                    _domain = (AssemblyDomain)AssemblyLoadContext.CurrentContextualReflectionContext;
                
                }


                if( _domain != DomainManagment.Default)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
#else
                if ( _domain != DomainManagment.Default)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
#endif
                return _domain;
            }
            set
            {

                _domain = value;

            }

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
            Exceptions.Add(exception);
            return exception;


        }


    }
}
