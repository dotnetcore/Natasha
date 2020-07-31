using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Domain.Template
{

    internal class UsingTemplate
    {

        internal readonly HashSet<string> _usings;
        internal readonly HashSet<Type> _usingTypes;

        public UsingTemplate()
        {

            _usings = new HashSet<string>();
            _usingTypes = new HashSet<Type>();

        }

        public UsingTemplate Using(string @using)
        {

            if (@using != default)
            {

                if (!_usings.Contains(@using))
                {

                    _usings.Add(@using);

                }

            }
            return this;

        }




        /// <summary>
        /// 从程序集里获取引用
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public UsingTemplate Using(Assembly assembly)
        {

            if (assembly != default)
            {
                Using(assembly.ExportedTypes);
            }
            return this;

        }



        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public UsingTemplate Using(params Assembly[] namespaces)
        {

            for (int i = 0; i < namespaces.Length; i++)
            {

                Using(namespaces[i]);

            }
            return this;

        }
        public UsingTemplate Using(IEnumerable<Assembly> namespaces)
        {

            foreach (var item in namespaces)
            {
                Using(item);
            }
            return this;

        }




        public UsingTemplate Using(IEnumerable<Type> namespaces)
        {

            foreach (var item in namespaces)
            {

                Using(item);

            }
            return this;

        }




        public UsingTemplate Using(Type type)
        {

            if (type != null && !_usingTypes.Contains(type))
            {

                _usingTypes.Add(type);
                return Using(type.Namespace);

            }
            return this;

        }

    }

}
