using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Domain.Template
{
    /// <summary>
    /// 引用模板
    /// </summary>
    internal class UsingRecoder
    {

        internal readonly HashSet<string> _usings;
        internal readonly HashSet<Type> _usingTypes;

        public UsingRecoder()
        {

            _usings = new HashSet<string>();
            _usingTypes = new HashSet<Type>();

        }

        public UsingRecoder Using(string @using)
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
        public UsingRecoder Using(Assembly assembly)
        {

            if (assembly != default)
            {
                try
                {
                    Using(assembly.ExportedTypes);
                }
                catch (Exception)
                {

                }
                
            }
            return this;

        }



        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public UsingRecoder Using(params Assembly[] namespaces)
        {

            for (int i = 0; i < namespaces.Length; i++)
            {

                Using(namespaces[i]);

            }
            return this;

        }
        public UsingRecoder Using(IEnumerable<Assembly> namespaces)
        {

            foreach (var item in namespaces)
            {
                Using(item);
            }
            return this;

        }




        public UsingRecoder Using(IEnumerable<Type> namespaces)
        {

            foreach (var item in namespaces)
            {

                Using(item);

            }
            return this;

        }




        public UsingRecoder Using(Type type)
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
