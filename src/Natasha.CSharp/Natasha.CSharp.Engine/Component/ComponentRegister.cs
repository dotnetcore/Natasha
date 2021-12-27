using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharpEngine;
using Natasha.Framework;
using System;
using System.Collections.Generic;

/// <summary>
/// 组件注册器
/// </summary>
public static class NatashaComponentRegister
{
    ///// <summary>
    ///// 注册编译器到编译引擎中
    ///// </summary>
    ///// <typeparam name="TCompiler"></typeparam>
    //public static void RegistCompiler<TCompiler>() where TCompiler : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
    //{
    //    CompilerComponent.RegisterDefault<TCompiler>();
    //}


    /// <summary>
    /// 注册域组件到编译引擎中
    /// </summary>
    /// <typeparam name="TDomain"></typeparam>
    /// <param name="initializeReference"></param>
    public static void RegistDomain<TDomain>(Func<string, bool> excludeReferencesFunc) where TDomain : DomainBase
    { 
        DomainComponent.RegisterDefault<TDomain>(excludeReferencesFunc);
    }


    ///// <summary>
    ///// 注册语法器到编译引擎中
    ///// </summary>
    ///// <typeparam name="TSyntax"></typeparam>
    //public static void RegistSyntax<TSyntax>() where TSyntax : SyntaxBase, new()
    //{
    //    SyntaxComponent.RegisterDefault<TSyntax>();
    //}

}

