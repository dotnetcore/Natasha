using System;

/// <summary>
/// 程序集编译构建器
/// </summary>
public partial class AssemblyCSharpBuilder 
{
    
    public AssemblyCSharpBuilder(string assemblyName = "")
    {
        _compileReferenceBehavior = LoadBehaviorEnum.UseBeforeIfExist;
        OutputFolder = GlobalOutputFolder;
        _compilerOptions = new(); 
        _semanticAnalysistor = new();
        SyntaxTrees = new();
        AssemblyName = assemblyName;
        if (AssemblyName == "")
        {
            AssemblyName = Guid.NewGuid().ToString("N");
        }
        DllFilePath = string.Empty;
        PdbFilePath = string.Empty;
        XmlFilePath = string.Empty;
    }

}



