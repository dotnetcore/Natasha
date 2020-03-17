using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Template
{
    public class OopFieldTemplate<T> : InheritanceTemplate<T> where T : OopFieldTemplate<T>, new()
    {
        public readonly StringBuilder OopFieldScript;
        public readonly HashSet<string> FieldsSet;

        public OopFieldTemplate()
        {
            OopFieldScript = new StringBuilder();
            FieldsSet = new HashSet<string>();
        }



        public T CreateField<S>(Action<FieldNameTemplate<S>> action) where S : FieldNameTemplate<S>, new()
        {
            var handler = new FieldNameTemplate<S>();
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
        public T CreateField(string access, Type type, string name, int? offset = null)
        {

            Using(type);
            if (!FieldsSet.Contains(name))
            {
                FieldsSet.Add(name);
            }


            if (offset != null)
            {
                OopFieldScript.AppendLine($"[FieldOffset({offset})]");
            }


            OopFieldScript.AppendLine($"{access} {type.GetDevelopName()} {name};");
            return Link;

        }
        public override T Builder()
        {
            base.Builder();
            _script.Append(OopFieldScript);
            return Link;
        }

        public T PublicField<S>(string name, int? offset = null)
        {
            return PublicField(typeof(S), name, offset);
        }

        public T PublicField(Type type, string name, int? offset = null)
        {
            return CreateField("public", type, name, offset);
        }

        public T PrivateField<S>(string name, int? offset = null)
        {
            return PrivateField(typeof(S), name, offset);
        }

        public T PrivateField(Type type, string name, int? offset = null)
        {
            return CreateField("private", type, name, offset);
        }

        public T InternalField<S>(string name, int? offset = null)
        {
            return InternalField(typeof(S), name, offset);
        }

        public T InternalField(Type type, string name, int? offset = null)
        {
            return CreateField("internal", type, name, offset);
        }

        public T ProtectedField<S>(string name, int? offset = null)
        {
            return ProtectedField(typeof(S), name, offset);
        }

        public T ProtectedField(Type type, string name, int? offset = null)
        {
            return CreateField("protected", type, name, offset);
        }

        public T PublicStaticField<S>(string name, int? offset = null)
        {
            return PublicStaticField(typeof(S), name, offset);
        }

        public T PublicStaticField(Type type, string name, int? offset = null)
        {
            return CreateField("public static", type, name, offset);
        }

        public T PrivateStaticField<S>(string name, int? offset = null)
        {
            return PrivateStaticField(typeof(S), name, offset);
        }

        public T PrivateStaticField(Type type, string name, int? offset = null)
        {
            return CreateField("private static", type, name, offset);
        }

        public T InternalStaticField<S>(string name, int? offset = null)
        {
            return InternalStaticField(typeof(S), name, offset);
        }

        public T InternalStaticField(Type type, string name, int? offset = null)
        {
            return CreateField("internal static", type, name, offset);
        }

        public T ProtectedStaticField<S>(string name, int? offset = null)
        {
            return ProtectedStaticField(typeof(S), name, offset);
        }

        public T ProtectedStaticField(Type type, string name, int? offset = null)
        {
            return CreateField("protected static", type, name, offset);
        }

        public T EnumField(string name, int value)
        {

            if (OopFieldScript.Length > 0)
            {
                OopFieldScript.AppendLine(",");
            }
            OopFieldScript.Append($"{name}={value}");
            return Link;

        }

        public T EnumField(string name)
        {

            if (OopFieldScript.Length > 0)
            {
                OopFieldScript.AppendLine(",");
            }
            OopFieldScript.Append(name);
            return Link;

        }

    }

}
