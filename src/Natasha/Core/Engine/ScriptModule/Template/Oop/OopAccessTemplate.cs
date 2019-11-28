using System;
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



        //[Obsolete("该方法已过时，请使用Public/Pirvate...属性")]
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




        public T Public
        {
            get { OopAccessScript = "public "; return Link; }
        }
        public T Private
        {
            get { OopAccessScript = "private "; return Link; }
        }
        public T Protected
        {
            get { OopAccessScript = "protected "; return Link; }
        }
        public T Internal
        {
            get { OopAccessScript = "internal "; return Link; }
        }
        public T ProtectedInternal
        {
            get { OopAccessScript = "protected internal "; return Link; }
        }





        public override T Builder()
        {

            base.Builder();
            _script.Append(OopAccessScript);
            return Link;

        }

    }

}
