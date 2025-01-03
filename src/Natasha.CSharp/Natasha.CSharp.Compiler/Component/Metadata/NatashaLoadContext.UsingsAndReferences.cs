﻿using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Component;
using Natasha.DynamicLoad.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public sealed partial class NatashaLoadContext
{

    internal void LoadMetadataWithAssembly(Assembly assembly)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly);
        if (result.HasValue)
        {
            AddReferenceAndUsingCode(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
        }
    }


    public NatashaLoadContext AddReferenceAndUsingCode(AssemblyName name, MetadataReference metadataReference, HashSet<string> usings, AssemblyCompareInformation compareInfomation = AssemblyCompareInformation.None)
    {
        ReferenceRecorder.AddReference(name, metadataReference, compareInfomation);
        UsingRecorder.Using(usings);
        return this;
    }


    /// <summary>
    /// 根据 实现程序集， 增加元数据 和 using
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public NatashaLoadContext AddReferenceAndUsingCode(Assembly assembly, Func<AssemblyName, bool>? excludeAssembliesFunc = null, AssemblyCompareInformation loadReferenceBehavior = AssemblyCompareInformation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly, null);
        if (result.HasValue)
        {
            AddReferenceAndUsingCode(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
            var assmblies = Creator.GetDependencyAssemblies(assembly);
            if (assmblies != null && assmblies.Any())
            {
                if (excludeAssembliesFunc != null)
                {
                    foreach (var depAssembly in assmblies)
                    {
                        var asmName = depAssembly.GetName();
                        if (!ReferenceRecorder.HasReference(asmName) && !excludeAssembliesFunc(asmName))
                        {
                            result = MetadataHelper.GetMetadataAndNamespaceFromMemory(depAssembly, null);
                            if (result.HasValue)
                            {
                                AddReferenceAndUsingCode(asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var depAssembly in assmblies)
                    {
                        var asmName = depAssembly.GetName();
                        if (!ReferenceRecorder.HasReference(asmName))
                        {
                            result = MetadataHelper.GetMetadataAndNamespaceFromMemory(depAssembly, null);
                            if (result.HasValue)
                            {
                                AddReferenceAndUsingCode(asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
                            }
                        }
                    }
                }

            }
        }
        return this;
    }

    /// <summary>
    /// 根据 引用程序集所在的文件， 增加元数据 和 using
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public NatashaLoadContext AddReferenceAndUsingCode(string path, AssemblyCompareInformation loadReferenceBehavior = AssemblyCompareInformation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromFile(path, null);
        if (result.HasValue)
        {
            AddReferenceAndUsingCode(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
        }
        return this;
    }
}