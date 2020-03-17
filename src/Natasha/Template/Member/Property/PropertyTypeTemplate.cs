using System;

namespace Natasha.Template
{
    public class PropertyTypeTemplate<T>: MemberModifierTemplate<T> where T : PropertyTypeTemplate<T>, new()
    {

        public string PropertyTypeScript;




        public T PropertyType(string name)
        {

            PropertyTypeScript = name;
            return Link;

        }




        public T PropertyType(Type type)
        {

            PropertyTypeScript = type.GetDevelopName();
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
