using System.Reflection;

namespace Natasha.Template
{
    /// <summary>
    /// 类型模板
    /// </summary>
    /// <typeparam name="T">LINK类型</typeparam>
    public class OopAccessTemplate<T> : OopAttributeTemplate<T>
    {

        public string OopAccessScript;


        public T OopAccess(MethodInfo reflectMethodInfo)
        {

            OopAccessScript = AccessReverser.GetAccess(reflectMethodInfo);
            return Link;

        }




        public T OopAccess(AccessTypes enumAccess)
        {

            OopAccessScript = AccessReverser.GetAccess(enumAccess);
            return Link;

        }




        public T OopAccess(string access)
        {

            OopAccessScript = access;
            return Link;

        }





        public override T Builder()
        {

            base.Builder();
            _script.Append(OopAccessScript);
            return Link;

        }

    }

}
