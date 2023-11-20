using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Using
{
    /// <summary>
    /// 引用模板
    /// </summary>
    public sealed class NatashaUsingCache
    {

        public readonly HashSet<string> _usings;
        //internal readonly HashSet<Type> _usingTypes;
        //private static readonly Regex _using_regex;

        //static NatashaUsingCache()
        //{
        //    _using_regex = new Regex("[a-zA-Z.]+")
        //}
        public NatashaUsingCache()
        {

            _usings = new HashSet<string>();

        }

        public int Count { get { return _usings.Count; } }



        public bool HasUsing(string @using)
        {
            return _usings.Contains(@using);
        } 



        public NatashaUsingCache Using(string? @using)
        {

            if (!string.IsNullOrEmpty(@using) && @using!.IndexOf('<') == -1)
            {

                _usings.Add(@using);

            }
            return this;

        }

        public NatashaUsingCache Using(IEnumerable<string> @using)
        {

            _usings.UnionWith(@using);
            return this;

        }




        /// <summary>
        /// 从程序集里获取引用
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        public NatashaUsingCache Using(Assembly assembly)
        {

            if (assembly != default)
            {
                try
                {
                    Using(assembly.GetExportedTypes());
                }
                catch 
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
        public NatashaUsingCache Using(params Assembly[] namespaces)
        {

            for (int i = 0; i < namespaces.Length; i++)
            {

                Using(namespaces[i]);

            }
            return this;

        }
        public NatashaUsingCache Using(IEnumerable<Assembly> namespaces)
        {

            foreach (var item in namespaces)
            {
                Using(item);
            }
            return this;

        }




        public NatashaUsingCache Using(IEnumerable<Type> namespaces)
        {

            foreach (var item in namespaces)
            {

                Using(item);

            }
            return this;

        }




        public NatashaUsingCache Using(Type type)
        {

            return Using(type.Namespace);

        }


        public override string ToString()
        {
            StringBuilder usings = new();
            foreach (var item in _usings)
            {
                usings.AppendLine($"using {item};");
            }
            return usings.ToString();
        }

    }

}