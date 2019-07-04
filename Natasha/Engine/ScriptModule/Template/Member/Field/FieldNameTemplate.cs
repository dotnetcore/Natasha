using System;
using System.Reflection;

namespace Natasha
{
    public class FieldNameTemplate<T>: MemberModifierTemplate<T>
    {
        public string FieldName;

        public FieldNameTemplate()
        {
           
        }


        public T Name(string name)
        {
            FieldName = name;
            return Link;
        }
        public T Name(Type type)
        {
            FieldName = type.GetDevelopName();
            return Link;
        }
        public T Name<S>()
        {
            return Name(typeof(S));
        }
        public T Name(MemberInfo info)
        {
            FieldName = info.Name;
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(FieldName);
            return Link;
        }
    }
}
