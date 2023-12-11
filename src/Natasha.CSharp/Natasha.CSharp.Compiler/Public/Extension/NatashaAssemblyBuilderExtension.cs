using Microsoft.CodeAnalysis;


public static class NatashaAssemblyBuilderExtension
{
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
}

