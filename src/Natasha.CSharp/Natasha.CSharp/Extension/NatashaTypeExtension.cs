namespace System
{
    public static class NatashaTypeExtension
    {


        public static NatashaReferenceDomain GetDomain(this Type type)
        {

            return type.Assembly.GetDomain();

        }



        public static void DisposeDomain(this Type type)
        {

            type.Assembly.DisposeDomain();

        }

    }
}
