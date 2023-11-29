using System;
using System.IO;

/// <summary>
/// 程序集编译构建器-输出
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    #region 输出设置相关
    public string AssemblyName;
    public string DllFilePath;
    public string? PdbFilePath;
    public string XmlFilePath;
    public string OutputFolder;
    /// <summary>
    /// 默认的输出文件夹
    /// </summary>
    public static readonly string GlobalOutputFolder;
    static AssemblyCSharpBuilder()
    {

        GlobalOutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "DynamicLibraryFolders");
        if (!Directory.Exists(GlobalOutputFolder))
        {
            Directory.CreateDirectory(GlobalOutputFolder);
        }

    }
    public AssemblyCSharpBuilder SetAssemblyName(string asmName)
    {
        AssemblyName = asmName;
        return this;
    }

    public AssemblyCSharpBuilder ConfigOutput(string dllFilePath, string pdbFilePath, string? xmlFilePath = null)
    {
        DllFilePath = dllFilePath;
        PdbFilePath = pdbFilePath;
        if (xmlFilePath != null)
        {
            XmlFilePath = xmlFilePath;
        }
        return this;
    }
    #endregion

}



