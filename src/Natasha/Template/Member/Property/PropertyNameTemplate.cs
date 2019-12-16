using System;
using System.Reflection;

namespace Natasha.Template
{
    public class PropertyNameTemplate<T>: PropertyTypeTemplate<T>
    {

        public string PropertyNameScript;


        public T PropertyName(string name)
        {

            PropertyNameScript = name;
            return Link;

        }




        public T PropertyName(MemberInfo info)
        {

            PropertyNameScript = info.Name;
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(PropertyNameScript);
            _script.Append("{get;set;}");
            return Link;

        }

    }

}
