using System;

namespace Natasha.Template
{
    public class PropertyTypeTemplate<T>: MemberModifierTemplate<T>
    {

        public string PropertyTypeScript;




        public T PropertyType(string name)
        {

            PropertyTypeScript = name;
            return Link;

        }




        public T PropertyType(Type type)
        {

            PropertyTypeScript = type.GetRuntimeName();
            return Link;

        }




        public T PropertyType<S>()
        {

            return PropertyType(typeof(S));

        }



        public override T Builder()
        {

            base.Builder();
            if (!PropertyTypeScript.EndsWith(" "))
            {
                PropertyTypeScript += " ";
            }
            _script.Append(PropertyTypeScript);
            return Link;

        }

    }

}
