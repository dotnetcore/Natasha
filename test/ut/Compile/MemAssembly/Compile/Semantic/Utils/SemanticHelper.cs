namespace MemAssembly.Compile.Semantic.Utils
{
    public static class SemanticHelper
    {
        public static (string id ,string script) GetScript(string script)
        {
            AssemblyCSharpBuilder builder = new();
            var compilation = builder
                .UseRandomLoadContext()
                .ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>())
                .Add(script)
                .GetAvailableCompilation();
            var diagnostics = builder.GetDiagnostics();
            compilation = builder
                .WithSemanticCheck()
                .GetAvailableCompilation();
            return (diagnostics!.Value[0].Id, compilation.SyntaxTrees[0].ToString());
        }
    }
}
