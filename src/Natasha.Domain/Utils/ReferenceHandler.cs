using Microsoft.CodeAnalysis;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Domain.Utils
{
    internal static class ReferenceHandler
    {

        /// <summary>
        /// 获取编译所需的引用库
        /// </summary>
        /// <returns></returns>
        internal static HashSet<PortableExecutableReference> GetCompileReferences(DomainBase main,DomainBase follow)
        {

            var defaultNode = (NatashaAssemblyDomain)main;

            //如果主从域不一样
            if (follow.Name != "Default")
            {

                //添加主域的主要引用
                var sets = new HashSet<PortableExecutableReference>(defaultNode.AssemblyReferencesCache.Values);

                //遍历主域的流引用集合
                foreach (var item in defaultNode.AssemblyReferencesCache.Keys)
                {

                    //遍历从域的流引用集合
                    foreach (var current in follow.AssemblyReferencesCache.Keys)
                    {

                        //如果程序集名相同
                        var defaultAssemblyName = item.GetName();
                        var currentAssemblyName = current.GetName();
                        if (defaultAssemblyName.Name == currentAssemblyName.Name)
                        {

                            //是否选用最新程序集
                            if (follow.UseNewVersionAssmebly)
                            {

                                //如果主域版本小,移除主域的引用
                                if (defaultAssemblyName.Version < currentAssemblyName.Version)
                                {

                                    //使用现有域的程序集版本
                                    sets.Remove(defaultNode.AssemblyReferencesCache[item]);
                                }
                            }
                            else
                            {
                                sets.Remove(defaultNode.AssemblyReferencesCache[item]);
                                
                            }
                            break;

                        }
                    }

                }

                //添加从域的流编译的引用
                sets.UnionWith(follow.AssemblyReferencesCache.Values);
                //添加主域的文件编译引用
                sets.UnionWith(defaultNode.OtherReferencesFromFile.Values);

                if (follow.OtherReferencesFromFile.Count > 0)
                {
                    //对比主从域的文件编译引用
                    foreach (var item in defaultNode.OtherReferencesFromFile.Keys)
                    {
                        foreach (var current in follow.OtherReferencesFromFile.Keys)
                        {
                            //如果从域指定了文件编译引用
                            if (item == current)
                            {
                                //移除主域的引用
                                sets.Remove(defaultNode.OtherReferencesFromFile[item]);
                            }
                        }
                    }
                }
                
                //添加从域的引用
                sets.UnionWith(follow.OtherReferencesFromFile.Values);
                return sets;

            }
            else
            {
                //如果是系统域则直接拼接自己的引用库
                var sets = new HashSet<PortableExecutableReference>(defaultNode.AssemblyReferencesCache.Values);
                sets.UnionWith(defaultNode.OtherReferencesFromFile.Values);
                return sets;
            }

        }
    }
}
