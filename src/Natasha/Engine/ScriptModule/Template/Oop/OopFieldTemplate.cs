using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Template
{
    public class OopFieldTemplate<T>:InheritanceTemplate<T>
    {
        public readonly StringBuilder OopFieldScript;
        internal readonly HashSet<string> _fieldsSet;

        public OopFieldTemplate()
        {
            OopFieldScript = new StringBuilder();
            _fieldsSet = new HashSet<string>();
        }



        public T CreateFiled(Action<FieldNameTemplate<T>> action)
        {
            var handler = new FieldNameTemplate<T>();
            action?.Invoke(handler);
            handler.Builder();
            OopFieldScript.Append(handler._script);
            return Link;
        }




        /// <summary>
        /// 定义一个字段
        /// </summary>
        /// <param name="access"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T CreateField(string access, Type type, string name)
        {
            Using(type);
            if (!_fieldsSet.Contains(name))
            {
                OopFieldScript.Append($"{access} {type.GetDevelopName()} {name};");
            }
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(OopFieldScript);
            return Link;
        }

        public T PublicField<S>(string name)
        {
            return PublicField(typeof(S), name);
        }

        public T PublicField(Type type, string name)
        {
            return CreateField("public", type, name);
        }

        public T PrivateField<S>(string name)
        {
            return PrivateField(typeof(S), name);
        }

        public T PrivateField(Type type, string name)
        {
            return CreateField("private", type, name);
        }

        public T InternalField<S>(string name)
        {
            return InternalField(typeof(S), name);
        }

        public T InternalField(Type type, string name)
        {
            return CreateField("internal", type, name);
        }

        public T ProtectedField<S>(string name)
        {
            return ProtectedField(typeof(S), name);
        }

        public T ProtectedField(Type type, string name)
        {
            return CreateField("protected", type, name);
        }

        public T PublicStaticField<S>(string name)
        {
            return PublicStaticField(typeof(S), name);
        }

        public T PublicStaticField(Type type, string name)
        {
            return CreateField("public static", type, name);
        }

        public T PrivateStaticField<S>(string name)
        {
            return PrivateStaticField(typeof(S), name);
        }

        public T PrivateStaticField(Type type, string name)
        {
            return CreateField("private static", type, name);
        }

        public T InternalStaticField<S>(string name)
        {
            return InternalStaticField(typeof(S), name);
        }

        public T InternalStaticField(Type type, string name)
        {
            return CreateField("internal static", type, name);
        }

        public T ProtectedStaticField<S>(string name)
        {
            return ProtectedStaticField(typeof(S), name);
        }

        public T ProtectedStaticField(Type type, string name)
        {
            return CreateField("protected static", type, name);
        }

    }

}
