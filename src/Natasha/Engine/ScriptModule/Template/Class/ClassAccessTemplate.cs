using System.Reflection;

namespace Natasha.Template
{
    /// <summary>
    /// 类型模板
    /// </summary>
    /// <typeparam name="T">LINK类型</typeparam>
    public class ClassAccessTemplate<T> : ClassAttributeTemplate<T>
    {

        public string ClassAccessScript;


        public T ClassAccess(MethodInfo reflectMethodInfo)
        {

            ClassAccessScript = AccessReverser.GetAccess(reflectMethodInfo);
            return Link;

        }




        public T ClassAccess(AccessTypes enumAccess)
        {

            ClassAccessScript = AccessReverser.GetAccess(enumAccess);
            return Link;

        }




        public T ClassAccess(string access)
        {

            ClassAccessScript = access;
            return Link;

        }





        public override T Builder()
        {

            base.Builder();
            _script.Append(ClassAccessScript);
            return Link;

        }

    }

}
