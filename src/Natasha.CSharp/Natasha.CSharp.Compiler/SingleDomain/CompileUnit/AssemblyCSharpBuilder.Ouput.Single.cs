#if !NETCOREAPP3_0_OR_GREATER
using System;
using System.IO;

/// <summary>
/// 程序集编译构建器-输出
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

// <summary>
    /// 将编译结果 (dll / pdb / xml) 文件写入此文件夹。
    /// 默认不输出到文件，只输出到内存。
    /// </summary>
    /// <param name="folder">文件夹路径</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithOutput(string? folder = null)
    {
        if (folder == null)
        {
            if (OutputFolder == GlobalOutputFolder)
            {
                OutputFolder = Path.Combine(GlobalOutputFolder,"Default");
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

}
#endif


