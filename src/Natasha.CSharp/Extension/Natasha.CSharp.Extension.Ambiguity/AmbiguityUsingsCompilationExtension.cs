using Natasha.CSharp;
using Natasha.CSharp.Builder;
using Natasha.CSharp.Extension.Ambiguity;
using Natasha.CSharp.Template;
using System.Collections.Generic;


public static class AmbiguityUsingsCompilationExtension
{
    public static NDelegate WithCS0104Handler(this NDelegate template)
    {
        template.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(AmbiguityUsings.NDelegateCreator(template));
        return template;
    }
    public static T WithCS0104Handler<T>(this MethodBuilder<T> builder) where T : MethodBuilder<T>, new()
    {
        builder.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(AmbiguityUsings.MethodBuilderCreator(builder));
        return builder.Link;
    }
    public static T WithCS0104Handler<T>(this OopBuilder<T> builder) where T : OopBuilder<T>, new()
    {
        builder.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(AmbiguityUsings.OopBuilderCreator(builder));
        return builder.Link;
    }

    public static T WithCS0104Handler<T>(this CompilerTemplate<T> template, params string[] usings) where T : CompilerTemplate<T>, new()
    {
        AmbiguityUsings._usingsCache[template.AssemblyBuilder.Compiler.Compilation] = new HashSet<string>(usings);
        template.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(AmbiguityUsings.UsingsCreator());
        return template.Link;
    }
}
