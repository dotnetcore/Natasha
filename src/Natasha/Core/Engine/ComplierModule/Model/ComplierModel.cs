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
        }




        public AssemblyDomain Domain
        {
            get
            {
                References.Clear();
                References.AddRange(DomainManagment.Default.ReferencesCache);
#if  !NETSTANDARD2_0
                bool isDefaultDomain = _domain == default && AssemblyLoadContext.CurrentContextualReflectionContext == default;
                if (isDefaultDomain)
                {

                    _domain = DomainManagment.Default;

                }
                else if (_domain == default && AssemblyLoadContext.CurrentContextualReflectionContext != null)
                {

                    _domain = (AssemblyDomain)(AssemblyLoadContext.CurrentContextualReflectionContext);

                }
                if (!isDefaultDomain)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
                return _domain;
#else
                bool isDefaultDomain = _domain == default;
                if (!isDefaultDomain)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
                return isDefaultDomain ? DomainManagment.Default : _domain;
#endif
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
