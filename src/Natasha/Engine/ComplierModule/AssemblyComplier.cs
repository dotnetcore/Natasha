using Natasha.Complier;

namespace Natasha
{
    public class AssemblyComplier : IComplier
    {
        public AssemblyComplier() { }
        public AssemblyComplier(string name) { Name = name; }




        public AssemblyDomain Domain
        {

            get { return ComplierInfos.Domain; }
            set { ComplierInfos.Domain = value; }

        }




        public CompilationException Add(string context)
        {

            var info = ComplierInfos.Add(context);
            Exception.ErrorFlag = info.ErrorFlag;
            return info;

        }




        public CompilationException Add(IScript template)
        {

            var info = ComplierInfos.Add(template.Script);
            Exception.ErrorFlag = info.ErrorFlag;
            return info;

        }

    }

}
