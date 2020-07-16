using Microsoft.CodeAnalysis;
using Natasha.CSharpEngine;
using Natasha.Framework;



public static class NatashaComponentRegister
{

    public static void RegistCompiler<TCompiler>() where TCompiler : CompilerBase<Compilation, CompilationOptions>, new()
    {
        CompilerManagement.RegisterDefault<TCompiler>();
    }

    public static void RegistDomain<TDomain>() where TDomain : DomainBase
    { 
        DomainManagement.RegisterDefault<TDomain>();
    }

    public static void RegistSyntax<TSyntax>() where TSyntax : SyntaxBase, new()
    {
        SyntaxManagement.RegisterDefault<TSyntax>();
    }

}

