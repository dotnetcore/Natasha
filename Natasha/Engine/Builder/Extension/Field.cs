using System;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        public LINK PublicField<T>(string name)
        {
            return PublicField(typeof(T), name);
        }

        public LINK PublicField(Type type, string name)
        {
            return Field("public", type, name);
        }

        public LINK PrivateField<T>(string name)
        {
            return PrivateField(typeof(T), name);
        }

        public LINK PrivateField(Type type, string name)
        {
            return Field("private", type, name);
        }

        public LINK InternalField<T>(string name)
        {
            return InternalField(typeof(T), name);
        }

        public LINK InternalField(Type type, string name)
        {
            return Field("internal", type, name);
        }

        public LINK ProtectedField<T>(string name)
        {
            return ProtectedField(typeof(T), name);
        }

        public LINK ProtectedField(Type type, string name)
        {
            return Field("protected", type, name);
        }

        public LINK PublicStaticField<T>(string name)
        {
            return PublicStaticField(typeof(T), name);
        }

        public LINK PublicStaticField(Type type, string name)
        {
            return Field("public static", type, name);
        }

        public LINK PrivateStaticField<T>(string name)
        {
            return PrivateStaticField(typeof(T), name);
        }

        public LINK PrivateStaticField(Type type, string name)
        {
            return Field("private static", type, name);
        }

        public LINK InternalStaticField<T>(string name)
        {
            return InternalStaticField(typeof(T), name);
        }

        public LINK InternalStaticField(Type type, string name)
        {
            return Field("internal static", type, name);
        }

        public LINK ProtectedStaticField<T>(string name)
        {
            return ProtectedStaticField(typeof(T), name);
        }

        public LINK ProtectedStaticField(Type type, string name)
        {
            return Field("protected static", type, name);
        }
    }
}
