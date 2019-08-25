using System;
using System.Reflection;

namespace Natasha.Template
{
    public class FieldNameTemplate<T>: FieldTypeTemplate<T>
    {

        public string FieldNameScript;


        public T FieldName(string name)
        {

            FieldNameScript = name;
            return Link;

        }




        public T FieldName(MemberInfo info)
        {

            FieldNameScript = info.Name;
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(FieldNameScript);
            _script.Append(';');
            return Link;

        }

    }

}
