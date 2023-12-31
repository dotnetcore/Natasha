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
    public string CommentFilePath;
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

    /// <summary>
    /// 设置程序编译后，dll/pdb/xml 文件的路径。使用此功能，程序编译后结果会输出到文件中。
    /// </summary>
    /// <param name="dllFilePath"></param>
    /// <param name="pdbFilePath"></param>
    /// <param name="commentFilePath"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithFileOutput(string dllFilePath, string? pdbFilePath = null, string? commentFilePath = null)
    {
        DllFilePath = dllFilePath;
        PdbFilePath = pdbFilePath;
        if (commentFilePath != null)
        {
            CommentFilePath = commentFilePath;
        }
        return this;
    }

    // <summary>
    /// 将编译结果 (dll / pdb / xml) 文件写入此文件夹。
    /// 默认不输出到文件，只输出到内存。
    /// </summary>
    /// <param name="folder">文件夹路径</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithFileOutput(string? folder = null)
    {
        if (folder == null)
        {
            if (OutputFolder == GlobalOutputFolder)
            {
                OutputFolder = Path.Combine(GlobalOutputFolder, "Default");
            }
        }
        else
        {
            OutputFolder = folder;
        }
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }
        DllFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.dll");
        PdbFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.pdb");
        CommentFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.xml");
        return this;
    }
    #endregion

}



