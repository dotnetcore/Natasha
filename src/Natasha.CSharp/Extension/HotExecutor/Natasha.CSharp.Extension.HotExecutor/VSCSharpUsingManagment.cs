using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Transactions;

namespace Natasha.CSharp.Extension.HotExecutor
{
    internal class VSCSharpUsingManagment
    {
        internal readonly ConcurrentDictionary<string, int> _usingCounter;
        internal readonly ConcurrentDictionary<string, HashSet<string>> _fileUsingMapper;
        private readonly List<UsingDirectiveSyntax> _usingNodes;
        private readonly object _lock = new();
        private bool _isChanged = false;
        public VSCSharpUsingManagment()
        {
            _usingCounter = [];
            _fileUsingMapper = [];
            _usingNodes = [];
        }

        public void ChangeUsing(string file, SyntaxNode root)
        {
            var newUsings = GetNamespaces(root);
            lock (_lock)
            {
                if (_fileUsingMapper.TryGetValue(file, out var usings))
                {
                    var distinctItems = newUsings.Union(usings).Except(newUsings.Intersect(usings));
                    if (distinctItems.Any())
                    {
                        _isChanged = true;
                    }
                    foreach (var @using in distinctItems)
                    {
                        if (_usingCounter.TryGetValue(@using, out int counter))
                        {
                            if (counter == 1)
                            {
                                _usingCounter.TryRemove(@using,out _);
                            }
                            else
                            {
                                _usingCounter[@using] = counter - 1;
                            }
                        }
                        else
                        {
                            _usingCounter[@using] = 1;
                        }
                    }
                }
                else
                {
                    _fileUsingMapper[file] = newUsings;
                    foreach (var @using in newUsings)
                    {
                        if (_usingCounter.TryGetValue(@using,out int counter))
                        {
                            counter += 1;
                        }
                        else
                        {
                            _isChanged = true;
                            _usingCounter[@using] = 1;
                        }
                    }
                }
            }
        }

        public void RemoveUsing(string file)
        {
            lock (_lock)
            {
                if (_fileUsingMapper.TryGetValue(file, out var usings))
                {
                    foreach (var @using in usings)
                    {
                        if (_usingCounter.TryGetValue(@using, out int counter))
                        {
                            if (counter == 1)
                            {
                                _isChanged = true;
                                _usingCounter.TryRemove(@using, out _);
                            }
                            else
                            {
                                _usingCounter[@using] = counter - 1;
                            }
                        }
                    }
                }
            }
        }

        public List<UsingDirectiveSyntax> GetUsingNodes()
        {
            lock (_lock)
            {
                if (_isChanged)
                {
                    _usingNodes.Clear();
                    foreach (var item in _usingCounter.Keys)
                    {
                        _usingNodes.Add(
                            SyntaxFactory.UsingDirective(
                                SyntaxFactory
                                .ParseName(item)
                                .WithLeadingTrivia(SyntaxFactory.Space))
                            .WithLeadingTrivia(SyntaxFactory.EndOfLine(Environment.NewLine)));
                    }
                    _isChanged = false;
                }
            }
            return _usingNodes;
        }

        static HashSet<string> GetNamespaces(SyntaxNode root)
        {
            var namespaces = new HashSet<string>();

            foreach (var node in root.DescendantNodes())
            {
                if (node is NamespaceDeclarationSyntax namespaceDeclaration)
                {
                    namespaces.Add(namespaceDeclaration.Name.ToString());
                }
            }

            return namespaces;
        }

        public IEnumerable<string> GetMainUsings()
        {
            return _usingCounter.Keys;
        }
    }
}
