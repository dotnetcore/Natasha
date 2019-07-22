using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class UsingTemplate<T>: TemplateRecoder<T>
    {
        public readonly StringBuilder UsingScript;
        private readonly HashSet<string> _usings;
        private readonly HashSet<Type> _usingTypes;
        public UsingTemplate()
        {
            UsingScript = new StringBuilder();
            _usings = new HashSet<string>();
            _usingTypes = new HashSet<Type>();
        }
       

        public T Using(string type)
        {
            if (type != null)
            {
                if (!_usings.Contains(type))
                {
                    _usings.Add(type);
                    UsingScript.Append($"using {type};");
                }
            }
            return Link;
        }


        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public T Using(params string[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return Link;
        }
        // <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">类型</param>
        /// <returns></returns>
        public T Using(params Type[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return Link;
        }
        public T Using(IEnumerable<Type> namespaces)
        {
            foreach (var item in namespaces)
            {
                Using(item);
            }
            return Link;
        }
        public T Using<S>()
        {
            Using(typeof(S));
            return Link;
        }
        public T Using(Type type)
        {
            if (type!=null && !_usingTypes.Contains(type))
            {
                _usingTypes.Add(type);
                Using(type.GetAllGenericTypes());
                return Using(type.Namespace);
            }
            return Link;
        }

        public T Using(MethodInfo info)
        {
            if (info != null)
            {
                Using(info.ReturnType);
                Using(info.GetParameters().Select(item => item.ParameterType));
            }
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            Using(UsingRecoder.Types);
            _script.Append(UsingScript);
            return Link;
        }
    }
}
