using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template
{

    public partial class UsingTemplate<T> : FlagTemplate<T> where T : UsingTemplate<T>, new()
    {

        public StringBuilder UsingScript;
        //internal readonly HashSet<string> _usings;

        public UsingTemplate()
        {

            UsingScript = new StringBuilder();
            //_usings = new HashSet<string>();

        }



        public T Using(NamespaceConverter converter)
        {

            if (converter.NamespaceAssembly != default)
            {

                Using(converter.NamespaceAssembly);

            }
            else if (converter.NamespaceType != default)
            {

                Using(converter.NamespaceType);

            }
            else if (converter.NamespaceString != default)
            {

                Using(converter.NamespaceString);

            }
            else if (converter.NamespaceAssemblys != default)
            {

                Using(converter.NamespaceAssemblys);

            }
            else if (converter.NamespaceStrings != default)
            {

                Using(converter.NamespaceStrings);

            }
            else if (converter.NamespaceTypes != default)
            {

                Using(converter.NamespaceTypes);

            }
            return Link;

        }



        public T Using(NamespaceConverter[]? converters)
        {

            if (converters != default)
            {

                for (int i = 0; i < converters.Length; i++)
                {

                    Using(converters[i]);

                }

            }
            return Link;

        }




        public T Using(string? @using)
        {
            UsingRecorder.Using(@using);
            return Link;

        }




        /// <summary>
        /// 从程序集里获取引用
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public T Using(Assembly assembly)
        {

            if (assembly != default)
            {
                Using(assembly.ExportedTypes);
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


        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public T Using(HashSet<string> namespaces)
        {

            foreach (var item in namespaces)
            {
                Using(item);
            }
            return Link;

        }

        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public T Using(params Assembly[] namespaces)
        {

            for (int i = 0; i < namespaces.Length; i++)
            {

                Using(namespaces[i]);

            }
            return Link;

        }
        public T Using(IEnumerable<Assembly> namespaces)
        {

            foreach (var item in namespaces)
            {
                Using(item);
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
            var types = type.GetAllTypes();
            foreach (var item in types)
            {
                Using(item.Namespace);
            }
            return Link;

        }




        public T Using(MethodInfo info)
        {

            Using(info.ReturnType);
            Using(info.GetParameters().Select(item => item.ParameterType));
            return Link;

        }



        public override T BuilderScript()
        {

            //  [{this}]
            //  [{Flag}]
            //  [namspace]
            //  { 
            //      [comment]
            //      [attribute]
            //      [access] [modifier] [name] [:interface] 
            //      {
            //          [body]
            //      }
            //      [otherBody]
            //  }
            base.BuilderScript();
            _script.Insert(0, GetUsingBuilder());
            return Link;

        }

    }

}
