using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharpEngine;
using Natasha.Framework;



public static class NatashaComponentRegister
{

    public static void RegistCompiler<TCompiler>() where TCompiler : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
    {
        CompilerComponent.RegisterDefault<TCompiler>();
    }

    public static void RegistDomain<TDomain>(bool initializeReference = true) where TDomain : DomainBase
    { 
        DomainComponent.RegisterDefault<TDomain>(initializeReference);
    }

    public static void RegistSyntax<TSyntax>() where TSyntax : SyntaxBase, new()
    {
        SyntaxComponent.RegisterDefault<TSyntax>();
    }

}

