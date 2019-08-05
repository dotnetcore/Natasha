using System;
using System.Reflection;

namespace Natasha.Template
{
    public class OopNameTemplate<T> : OopTypeTemplate<T>
    {

        public string OopNameScript;



        public OopNameTemplate()
        {

            OopNameScript = "N" + Guid.NewGuid().ToString("N");

        }




        




        public T OopName(string name)
        {

            OopNameScript = name;
            return Link;

        }




        public T OopName(Type type)
        {

            OopNameScript = type.GetDevelopName();
            return Link;

        }




        public T OopName<S>()
        {

            return OopName(typeof(S));

        }




        public T OopName(MethodInfo reflectMethodInfo)
        {

            OopNameScript = reflectMethodInfo.Name;
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(OopNameScript);
            return Link;

        }

    }

}
