using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Standard
{
    public abstract class ScriptStandard<LINK>
    {
        internal StringBuilder _using;
        internal StringBuilder _script;
        internal StringBuilder _inheritance;
        internal HashSet<string> _usings;
        internal string _class_name;
        internal string _text;
        internal LINK _link;
        internal string _namespace;

        public ScriptStandard()
        {
            _class_name = "N" + Guid.NewGuid().ToString("N");
            _using = new StringBuilder();
            _usings = new HashSet<string>();
            _script = new StringBuilder();
            _inheritance = new StringBuilder();
        }


        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public LINK Class(string @class)
        {
            _class_name = @class;
            return _link;
        }


        /// <summary>
        /// 写内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public LINK Body(string text)
        {
            _text = text;
            return _link;
        }

        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <returns></returns>
        public LINK Namespace(string @namespace)
        {
            _namespace = @namespace;
            return _link;
        }
        public LINK Namespace<T>()
        {
            return Namespace(typeof(T));
        }
        public LINK Namespace(Type type)
        {
            _namespace = type.Namespace;
            return _link;
        }

        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public LINK Using(params string[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return _link;
        }
        // <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">类型</param>
        /// <returns></returns>
        public LINK Using(params Type[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return _link;
        }
        public LINK Using<T>()
        {
            Using(typeof(T));
            return _link;
        }
        public LINK Using(Type type)
        {
            if (type==null)
            {
                return _link;
            }
            return Using(type.Namespace);
        }
        public LINK Using(string type)
        {
            if (type != null)
            {
                if (!_usings.Contains(type))
                {
                    _usings.Add(type);
                    _using.Append($"using {type};");
                }
            }
            return _link;
        }

        /// <summary>
        /// 设置继承
        /// </summary>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public LINK Inheritance(params string[] types)
        {
            if (types != null && types.Length > 0)
            {
                if (_inheritance.Length == 0)
                {
                    _inheritance.Append(":");
                    _inheritance.Append(types[0]);
                    for (int i = 1; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i]}");
                    }
                }
                else
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i]}");
                    }
                }
            }
            Using(types);
            return _link;
        }
        public LINK Inheritance(params Type[] types)
        {
            if (types != null && types.Length > 0)
            {
                if (_inheritance.Length == 0)
                {
                    _inheritance.Append(":");
                    _inheritance.Append(types[0].Name);
                    for (int i = 1; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i].Name}");
                    }
                }
                else
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i].Name}");
                    }
                }
            }
            Using(types);
            return _link;
        }
        public LINK Inheritance<T>()
        {
            if (_inheritance.Length == 0 )
            {
                _inheritance.Append(":");
                _inheritance.Append(typeof(T).Name); 
            }
            else
            {
                _inheritance.Append($",{typeof(T).Name}");
            }
            Using<T>();
            return _link;
        }

        


        public LINK MakerHeader()
        {
            _script.Append(_using);
            if (_namespace != null)
            {
                _script.Append($"namespace {_namespace}{{");
            }
            _script.Append($"public class {_class_name} {_inheritance}");
            return _link;
        }

        public LINK MakerStaticHeader()
        {
            _script.Append(_using);
            if (_namespace != null)
            {
                _script.Append($"namespace {_namespace}{{");
            }
            _script.Append($"public static class {_class_name}");
            return _link;
        }


        public LINK MakerContent(string content)
        {
            _script.Append($"{{{content}}}");
            if (_namespace != null)
            {
                _script.Append("}");
            }
            return _link;
        }

        public string Script
        {
            get { return _script.ToString(); }
        }
    }
}
