using System;
using System.Reflection;

namespace Natasha.Template
{
    public class ClassNameTemplate<T> : ClassModifierTemplate<T>
    {

        public string ClassNameScript;
        public bool IsStruct;


        public ClassNameTemplate()
        {

            ClassNameScript = "N" + Guid.NewGuid().ToString("N");
            IsStruct = false;

        }




        public T ChangeToStruct()
        {

            IsStruct = true;
            return Link;

        }




        public T ClassName(string name)
        {

            ClassNameScript = name;
            return Link;

        }




        public T ClassName(Type type)
        {

            ClassNameScript = type.GetDevelopName();
            return Link;

        }




        public T ClassName<S>()
        {

            return ClassName(typeof(S));

        }




        public T ClassName(MethodInfo reflectMethodInfo)
        {

            ClassNameScript = reflectMethodInfo.Name;
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(IsStruct ? "struct" : "class " +ClassNameScript);
            return Link;

        }

    }

}
