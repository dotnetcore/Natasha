using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Template
{
    public class OopPropertyTemplate<T> : OopFieldTemplate<T> where T : OopPropertyTemplate<T>, new()
    {
        public readonly StringBuilder OopPropertyScript;
        internal readonly HashSet<string> _propertiesSet;

        public OopPropertyTemplate()
        {
            OopPropertyScript = new StringBuilder();
            _propertiesSet = new HashSet<string>();
        }



        public T CreateProperty<S>(Action<PropertyNameTemplate<S>> action) where S : PropertyNameTemplate<S>, new()
        {
            var handler = new PropertyNameTemplate<S>();
            action?.Invoke(handler);
            handler.Builder();
            OopPropertyScript.Append(handler._script);
            return Link;
        }




        /// <summary>
        /// 定义一个字段
        /// </summary>
        /// <param name="access"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T CreateProperty(string access, Type type, string name)
        {
            Using(type);
            if (!_propertiesSet.Contains(name))
            {
                OopPropertyScript.AppendLine($"{access} {type.GetDevelopName()} {name}{{get;set;}}");
            }
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(OopPropertyScript);
            return Link;
        }

        public T PublicProperty<S>(string name)
        {
            return PublicProperty(typeof(S), name);
        }

        public T PublicProperty(Type type, string name)
        {
            return CreateProperty("public", type, name);
        }

        public T PrivateProperty<S>(string name)
        {
            return PrivateProperty(typeof(S), name);
        }

        public T PrivateProperty(Type type, string name)
        {
            return CreateProperty("private", type, name);
        }

        public T InternalProperty<S>(string name)
        {
            return InternalProperty(typeof(S), name);
        }

        public T InternalProperty(Type type, string name)
        {
            return CreateProperty("internal", type, name);
        }

        public T ProtectedProperty<S>(string name)
        {
            return ProtectedProperty(typeof(S), name);
        }

        public T ProtectedProperty(Type type, string name)
        {
            return CreateProperty("protected", type, name);
        }

        public T PublicStaticProperty<S>(string name)
        {
            return PublicStaticProperty(typeof(S), name);
        }

        public T PublicStaticProperty(Type type, string name)
        {
            return CreateProperty("public static", type, name);
        }

        public T PrivateStaticProperty<S>(string name)
        {
            return PrivateStaticProperty(typeof(S), name);
        }

        public T PrivateStaticProperty(Type type, string name)
        {
            return CreateProperty("private static", type, name);
        }

        public T InternalStaticProperty<S>(string name)
        {
            return InternalStaticProperty(typeof(S), name);
        }

        public T InternalStaticProperty(Type type, string name)
        {
            return CreateProperty("internal static", type, name);
        }

        public T ProtectedStaticProperty<S>(string name)
        {
            return ProtectedStaticProperty(typeof(S), name);
        }

        public T ProtectedStaticProperty(Type type, string name)
        {
            return CreateProperty("protected static", type, name);
        }

        public T EnumProperty(string name, int value)
        {

            if (OopPropertyScript.Length > 0)
            {
                OopPropertyScript.AppendLine(",");
            }
            OopPropertyScript.Append($"{name}={value}");
            return Link;

        }

        public T EnumProperty(string name)
        {

            if (OopPropertyScript.Length > 0)
            {
                OopPropertyScript.AppendLine(",");
            }
            OopPropertyScript.Append(name);
            return Link;

        }

    }

}
