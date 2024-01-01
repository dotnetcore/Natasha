using Natasha.CSharp.Template;

namespace Natasha.CSharp
{

    public static class OopDefinedTypeExtension
    {

        public static T Record<T>(this DefinedTypeTemplate<T> defined) where T : DefinedTypeTemplate<T>, new()
        {

            return defined.Type("record");

        }

        public static T Class<T>(this DefinedTypeTemplate<T> defined) where T : DefinedTypeTemplate<T>, new()
        {

            return defined.Type("class");

        }




        public static T Struct<T>(this DefinedTypeTemplate<T> defined) where T : DefinedTypeTemplate<T>, new()
        {

            return defined.Type("struct");

        }




        public static T Interface<T>(this DefinedTypeTemplate<T> defined) where T : DefinedTypeTemplate<T>, new()
        {

            return defined.Type("interface");

        }




        public static T Enum<T>(this DefinedTypeTemplate<T> defined) where T : DefinedTypeTemplate<T>, new()
        {

            return defined.Type("enum");

        }


    }

}
