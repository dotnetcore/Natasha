using Microsoft.CodeAnalysis;


public static class NatashaAssemblyBuilderExtension
{

    public static AssemblyCSharpBuilder EnableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.ConfigCompilerOption(opt => opt.SetNullableCompile(NullableContextOptions.Enable));
        return builder;
    }
    public static AssemblyCSharpBuilder DisableNullableCompile(this AssemblyCSharpBuilder builder)
    {
        builder.ConfigCompilerOption(opt => opt.SetNullableCompile(NullableContextOptions.Disable));
        return builder;
    }

    public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder, string folder)
    {
        builder.OutputFolder = folder;
        return builder;
    }
    public static AssemblyCSharpBuilder SetDllFilePath(this AssemblyCSharpBuilder builder, string dllFilePath)
    {
        builder.DllFilePath = dllFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetPdbFilePath(this AssemblyCSharpBuilder builder, string pdbFilePath)
    {
        builder.PdbFilePath = pdbFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetXmlFilePath(this AssemblyCSharpBuilder builder, string xmlFilePath)
    {
        builder.XmlFilePath = xmlFilePath;
        return builder;
    }

    public static AssemblyCSharpBuilder DisableSemanticCheck(this AssemblyCSharpBuilder builder)
    {
        builder.EnableSemanticHandler = false;
        return builder;
    }
    public static AssemblyCSharpBuilder EnableSemanticCheck(this AssemblyCSharpBuilder builder)
    {
        builder.EnableSemanticHandler = true;
        return builder;
    }
}

