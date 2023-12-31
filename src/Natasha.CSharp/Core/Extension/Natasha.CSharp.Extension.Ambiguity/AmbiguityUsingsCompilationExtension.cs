using Natasha.CSharp.Builder;
using Natasha.CSharp.Extension.Ambiguity;


public static class AmbiguityUsingsCompilationExtension
{
    public static NDelegate WithCS0104Handler(this NDelegate template)
    {
        template.AssemblyBuilder.AddSemanticAnalysistor(AmbiguityUsings.NDelegateCreator(template));
        return template;
    }
    public static T WithCS0104Handler<T>(this MethodBuilder<T> builder) where T : MethodBuilder<T>, new()
    {
        builder.AssemblyBuilder.AddSemanticAnalysistor(AmbiguityUsings.MethodBuilderCreator(builder));
        return builder.Link!;
    }
    public static T WithCS0104Handler<T>(this OopBuilder<T> builder) where T : OopBuilder<T>, new()
    {
        builder.AssemblyBuilder.AddSemanticAnalysistor(AmbiguityUsings.OopBuilderCreator(builder));
        return builder.Link!;
    }

    //public static T WithCS0104Handler<T>(this CompilerTemplate<T> template, params string[] usings) where T : CompilerTemplate<T>, new()
    //{
    //    AmbiguityUsings._usingsCache[template.AssemblyBuilder.Compiler.Compilation!] = new HashSet<string>(usings);
    //    template.AssemblyBuilder.AddSemanticAnalysistor(AmbiguityUsings.UsingsCreator());
    //    return template.Link!;
    //}
}
