using System;

namespace Natasha.Template
{
    public class FieldTypeTemplate<T>: MemberModifierTemplate<T>
    {

        public string FieldTypeScript;




        public T FieldType(string name)
        {

            FieldTypeScript = name;
            return Link;

        }




        public T FieldType(Type type)
        {

            FieldTypeScript = type.GetDevelopName();
            return Link;

        }




        public T FieldType<S>()
        {

            return FieldType(typeof(S));

        }



        public override T Builder()
        {

            base.Builder();
            if (!FieldTypeScript.EndsWith(" "))
            {
                FieldTypeScript += " ";
            }
            _script.Append(FieldTypeScript);
            return Link;

        }

    }

}
