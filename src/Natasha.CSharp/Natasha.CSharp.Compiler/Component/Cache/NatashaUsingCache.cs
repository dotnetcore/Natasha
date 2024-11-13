using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Natasha.CSharp.Compiler.Component
{
    /// <summary>
    /// 引用模板
    /// </summary>
    public sealed class NatashaUsingCache : IEnumerable<string>, IDisposable
    {
        private bool _changed;
        public readonly HashSet<string> _usings;
        private List<UsingDirectiveSyntax> _usingNodes;
        private string _usingScript = string.Empty;
        public int Count { get { return _usings.Count; } }

        public NatashaUsingCache()
        {
            _usingNodes = [];
            _usings = [];

        }

        private void ApplyIfChanged()
        {
            lock (_usings)
            {
                if (_changed)
                {
                    StringBuilder usings = new();
                    foreach (var item in _usings)
                    {
                        usings.AppendLine($"using {item};");
                    }
                    _usingScript = usings.ToString();
                    _usingNodes.Clear();
                    _usingNodes.AddRange(_usings.Select(item =>
                    SyntaxFactory.UsingDirective(
                        SyntaxFactory.ParseName(item)
                        .WithLeadingTrivia(SyntaxFactory.Space)
                    ).WithLeadingTrivia(SyntaxFactory.EndOfLine(Environment.NewLine))));
                    _changed = false;
                }
            }
        }

        public override string ToString()
        {
            ApplyIfChanged();
            return _usingScript;
        }

        public List<UsingDirectiveSyntax> ToUsingNodes()
        {
            ApplyIfChanged();
            return _usingNodes;
        }

        public bool HasUsing(string @using)
        {
            lock (_usings)
            {
                return _usings.Contains(@using);
            }
        } 



        public NatashaUsingCache Using(string? @using)
        {

            if (!string.IsNullOrEmpty(@using) && @using!.IndexOf('<') == -1)
            {
                lock (_usings)
                {
                    if (_usings.Add(@using))
                    {
                        _changed = true;
                    }
                }
            }
            return this;

        }

        public NatashaUsingCache Using(IEnumerable<string> @using)
        {
            if (@using.Any())
            {
                lock (_usings)
                {
                    _usings.UnionWith(@using);
                    _changed = true;
                }
            }
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

        public NatashaUsingCache Remove(string @using)
        {
            lock (_usings)
            {
                if (_usings.Remove(@using))
                {
                    _changed = true;
                }
            }
            return this;
        }

        public NatashaUsingCache Remove(IEnumerable<string> @usings)
        {
            if (@usings.Any())
            {
                lock (_usings)
                {
                    _usings.ExceptWith(@usings);
                    _changed = true;
                }
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
            if (type.IsPublic || type.IsNestedPublic)
            {
                return Using(type.Namespace);
            }
            return this;
        }

        /// <summary>
        /// 根据当前 using 缓存以及需要被排除的 using code , 返回一个新的 using 缓存.
        /// </summary>
        /// <param name="exceptUsings">从当前 using 中排除的 Using Code</param>
        /// <returns></returns>
        public NatashaUsingCache WithExceptUsing(IEnumerable<string> exceptUsings)
        {
            NatashaUsingCache newCache = new();
            newCache._usings.UnionWith(_usings);
            newCache._usings.ExceptWith(exceptUsings);
            newCache._changed = true;
            return newCache;
        }


        /// <summary>
        /// 使用当前 using 缓存包装脚本
        /// </summary>
        /// <param name="script">需要被包装的脚本</param>
        /// <returns></returns>
        public string WrapperScript(string script)
        {
            lock (_usings)
            {
                return $"{ToString()}{Environment.NewLine}{script}";
            }
        }


        public IEnumerator<string> GetEnumerator()
        {
            return _usings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in _usings)
            {
                yield return item;
            }
        }

        public void Dispose()
        {
            _usings.Clear();
            _usingNodes.Clear();
            _usingScript = string.Empty;
        }
    }

}