using Natasha.Template;
using System;

namespace Natasha
{

    public static class MethodForDefinedTypeExtension
    {

        public static T Return<T>(this DefinedTypeTemplate<T> defined,Type type) where T : DefinedTypeTemplate<T>, new()
        {
            return defined.DefinedType(type);
        }

    }

}
