using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharpEngine;
using Natasha.Framework;



public static class ComponentRegister
{

    public static void RegistCompiler<TCompiler>() where TCompiler : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
    {
        CompilerManagement.RegisterDefault<TCompiler>();
    }

    public static void RegistDomain<TDomain>() where TDomain : DomainBase
    { 
        DomainManagement.RegisterDefault<TDomain>();
    }

    public static void RegisteSyntax<TSyntax>() where TSyntax : SyntaxBase, new()
    {
        SyntaxManagement.RegisterDefault<TSyntax>();
    }

}

