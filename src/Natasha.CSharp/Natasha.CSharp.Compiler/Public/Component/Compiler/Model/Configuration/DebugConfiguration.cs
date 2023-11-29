
using Microsoft.CodeAnalysis.Emit;

public class DebugConfiguration
{
    
    internal DebugInformationFormat _informationFormat = DebugInformationFormat.PortablePdb;

    /// <summary>
    /// 采用文件加载方式搜集源代码信息
    /// </summary>
    /// <returns></returns>
    public DebugConfiguration WriteToFile()
    {
        _informationFormat = DebugInformationFormat.PortablePdb;
        return this;
    }
    /// <summary>
    /// 将Pdb输出到程序集中，并直接从程序集加载Pdb调试信息
    /// </summary>
    /// <returns></returns>
    public DebugConfiguration WriteToAssembly()
    {
        _informationFormat = DebugInformationFormat.Embedded;
        return this;
    }
    
}

