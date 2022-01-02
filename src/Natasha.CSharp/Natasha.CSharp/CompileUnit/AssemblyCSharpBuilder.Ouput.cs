using System;
using System.IO;

/// <summary>
/// 程序集编译构建器-输出
/// </summary>
public partial class AssemblyCSharpBuilder 
{

    #region 输出设置相关
    public string AssemblyName;
    public string DllFilePath;
    public string PdbFilePath;
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
    public void UseNatashaFileOut()
    {
        if (OutputFolder == GlobalOutputFolder)
        {
            OutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
        }
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }
        DllFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.dll");
        PdbFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.pdb");
        XmlFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.xml");
    }
    #endregion


}



