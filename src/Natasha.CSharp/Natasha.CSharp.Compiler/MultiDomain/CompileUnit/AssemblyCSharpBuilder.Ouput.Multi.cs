#if NETCOREAPP3_0_OR_GREATER
using System.IO;

/// <summary>
/// 程序集编译构建器-输出
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    /// <summary>
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
                OutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
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
        XmlFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.xml");
        return this;
    }
}
#endif


