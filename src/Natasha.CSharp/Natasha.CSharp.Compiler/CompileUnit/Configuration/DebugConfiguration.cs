using Microsoft.CodeAnalysis.Emit;

public class DebugConfiguration
{
    
    internal DebugInformationFormat? _informationFormat;

    /// <summary>
    /// 采用文件加载方式搜集源代码信息, 适用于 CoreCLR & Mono
    /// </summary>
    /// <returns></returns>
    public DebugConfiguration ForCore()
    {
        _informationFormat = DebugInformationFormat.PortablePdb;
        return this;
    }
    /// <summary>
    /// 用文件加载方式搜集源代码信息, 适用于 Standard
    /// </summary>
    /// <returns></returns>
    public DebugConfiguration ForStandard()
    {
        _informationFormat = DebugInformationFormat.Pdb;
        return this;
    }
    /// <summary>
    /// 将 Pdb 输出到程序集中，并直接从程序集加载 Pdb 调试信息
    /// </summary>
    /// <returns></returns>
    public DebugConfiguration ForAssembly()
    {
        _informationFormat = DebugInformationFormat.Embedded;
        return this;
    }
    
}

