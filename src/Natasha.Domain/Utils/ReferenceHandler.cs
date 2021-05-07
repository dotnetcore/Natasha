using Microsoft.CodeAnalysis;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Domain.Utils
{
    public static class ReferenceHandler
    {

        /// <summary>
        /// 获取编译所需的引用库
        /// </summary>
        /// <returns></returns>
        public static HashSet<PortableExecutableReference> GetCompileReferences(DomainBase main,DomainBase follow)
        {

            var defaultNode = (NatashaAssemblyDomain)main;

            //如果主从域不一样
            if (follow.Name != "default")
            {

                //去除剩余的部分
                var sets = new HashSet<PortableExecutableReference>(defaultNode.ReferencesFromStream.Values);

                //遍历主域的流引用集合
                foreach (var item in defaultNode.ReferencesFromStream.Keys)
                {
                    //遍历从域的流引用集合
                    foreach (var current in follow.ReferencesFromStream.Keys)
                    {

                        //如果程序集名相同
                        if (item.GetName().Name == current.GetName().Name)
                        {

                            //是否选用最新程序集
                            if (follow.UseNewVersionAssmebly)
                            {

                                //如果主域版本小,移除主域的引用
                                if (item.GetName().Version < current.GetName().Version)
                                {

                                    //使用现有域的程序集版本
                                    sets.Remove(defaultNode.ReferencesFromStream[item]);
                                    break;

                                }
                            }
                            else
                            {
                                sets.Remove(defaultNode.ReferencesFromStream[item]);
                                break;
                            }
                        }
                    }
                }

                //添加从域的流编译的引用
                sets.UnionWith(follow.ReferencesFromStream.Values);
                //添加主域的文件编译引用
                sets.UnionWith(defaultNode.ReferencesFromFile.Values);

                //对比主从域的文件编译引用
                foreach (var item in defaultNode.ReferencesFromFile.Keys)
                {
                    foreach (var current in follow.ReferencesFromFile.Keys)
                    {
                        //如果从域指定了文件编译引用
                        if (item == current)
                        {
                            //移除主域的引用
                            sets.Remove(defaultNode.ReferencesFromFile[item]);
                        }
                    }
                }

                //添加从域的引用
                sets.UnionWith(follow.ReferencesFromFile.Values);
                return sets;

            }
            else
            {
                //如果是系统域则直接拼接自己的引用库
                var sets = new HashSet<PortableExecutableReference>(defaultNode.ReferencesFromStream.Values);
                sets.UnionWith(defaultNode.ReferencesFromFile.Values);
                return sets;
            }

        }
    }
}
