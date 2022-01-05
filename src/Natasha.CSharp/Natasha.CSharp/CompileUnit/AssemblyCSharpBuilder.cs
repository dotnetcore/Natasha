using Natasha.CSharp.Compiler.SemanticAnalaysis;
using System;

/// <summary>
/// 程序集编译构建器
/// </summary>
public partial class AssemblyCSharpBuilder 
{
    
    public AssemblyCSharpBuilder(string assemblyName = "")
    {
        
        EnableSemanticHandler = true;
        _compileReferenceBehavior = LoadBehaviorEnum.UseDefault;
        OutputFolder = GlobalOutputFolder;
        _compilerOptions = new(); 
        _semanticAnalysistor = new();
        _semanticAnalysistor.Add(UsingAnalysistor._usingSemanticDelegate);
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



