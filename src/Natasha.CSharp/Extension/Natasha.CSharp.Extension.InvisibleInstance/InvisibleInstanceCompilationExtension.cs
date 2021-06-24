using Natasha.CSharp.Template;

public static class InvisibleInstanceCompilationExtension
{
    public static T WithFirstArgInvisible<T>(this CompilerTemplate<T> template, string argument) where T : CompilerTemplate<T>, new()
    {
        template.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(InvisibleInstance.Creator(argument));
        return template.Link;
    }
    public static T WithFirstArgInvisible<T>(this CompilerTemplate<T> template) where T : CompilerTemplate<T>, new()
    {
        template.AssemblyBuilder.Compiler.AppendSemanticAnalysistor(InvisibleInstance.Creator());
        return template.Link;
    }
}

