using Natasha;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    public class MethodBuilder<T> :IComplier
    {
        //private readonly static Regex _get_class;
        protected T _link;
        static MethodBuilder()
        {
            //_get_class = new Regex(@"\sclass.*?(?<result>[a-zA-Z0-9]*?)[\s]*{", RegexOptions.Compiled | RegexOptions.Singleline);
        }

        internal (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) _info;
        public ClassBuilder ClassTemplate;
        public MethodTemplate MethodTemplate;
        public MethodBuilder()
        {
            ClassTemplate = new ClassBuilder();
            MethodTemplate = new MethodTemplate();
        }
        
        public string MethodScript
        {
            get { return _info.Script; }
        }

        public virtual T SetClassTemplate(ClassBuilder template)
        {
            ClassTemplate = template;
            return _link;
        }

        public virtual T UseClassTemplate(Action<ClassBuilder> template)
        {
            template(ClassTemplate);
            return _link;
        }

        public virtual T SetBodyTemplate(MethodTemplate template)
        {
            MethodTemplate = template;
            return _link;
        }
        public virtual T UseBodyTemplate(Action<MethodTemplate> template)
        {
            template(MethodTemplate);
            return _link;
        }


        public override string Builder()
        {
            _info = MethodTemplate.Package();
            ClassTemplate.Using(_info.Types);
            return ClassTemplate.ClassBody(_info.Script).Builder();
        }
        public override Delegate Complie()
        {
            
            Assembly assembly = GetAssemblyByScript(ClassTemplate.NameScript);

            if (assembly == null)
            {
                return null;
            }

            return AssemblyOperator
                .Loader(assembly)[ClassTemplate.NameScript]
                .GetMethod(_info.Flag)
                .CreateDelegate(_info.Delegate);
        }

    }
}
