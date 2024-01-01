using Natasha.CSharp.Extension.Ambiguity;

public static class AssenblyCSharpBuilder
{
    public static AssemblyCSharpBuilder WithCodecov(this AssemblyCSharpBuilder builder)
    {
        builder.AddSemanticAnalysistor(AmbiguityUsings.SemanticAction);
        return builder;
    }
}

