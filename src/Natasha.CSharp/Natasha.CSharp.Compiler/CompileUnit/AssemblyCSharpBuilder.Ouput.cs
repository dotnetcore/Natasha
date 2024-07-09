using Natasha.CSharp.Compiler.Utils;
using System;
using System.IO;
using System.Reflection;

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
    private bool _cleanOutput;
    /// <summary>
    /// 重复编译时，强制清除已存在的文件.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithForceCleanOutput()
    {
        _cleanOutput = true;
        return this;
    }
    /// <summary>
    /// 重复编译时，不清除已存在的文件. 使用 old -> repeate.guid.oldname 进行替换.(不指定默认使用该方案)
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithoutForceCleanOutput()
    {
        _cleanOutput = false;
        return this;
    }
    /// <summary>
    /// 设置程序集名称.
    /// </summary>
    /// <param name="asmName">程序集名</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder SetAssemblyName(string asmName)
    {
        AssemblyName = asmName;
        return this;
    }
    /// <summary>
    /// 程序只编译不输出文件.
    /// </summary>
    /// <remarks>
    /// 注：该逻辑会被缓存，复用时无需重复添加方法.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithoutFileOutput()
    {
        DllFilePath = string.Empty;
        PdbFilePath = null;
        CommentFilePath = string.Empty;
        return this;
    }

    /// <summary>
    /// 设置程序编译后，dll/pdb/xml 文件的路径. 使用此功能，程序编译后结果会输出到文件中.
    /// </summary>
    /// <remarks>
    /// 注：该逻辑 [不会] 被缓存，复用时 [需重] 再次调用该方法.
    /// </remarks>
    /// <param name="dllFilePath">绝对路径，例如："c:\1.dll"</param>
    /// <param name="pdbFilePath">绝对路径，例如："c:\1.pdb"</param>
    /// <param name="commentFilePath">绝对路径，例如："c:\1.xml"</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithFileOutput(string dllFilePath, string? pdbFilePath = null, string? commentFilePath = null)
    {
        if (dllFilePath!=string.Empty)
        {
            var folder = Path.GetDirectoryName(dllFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            DllFilePath = dllFilePath;
        }
       
        if (pdbFilePath != null)
        {
            var folder = Path.GetDirectoryName(pdbFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
        PdbFilePath = pdbFilePath;
        

        if (commentFilePath != null)
        {
            var folder = Path.GetDirectoryName(commentFilePath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            CommentFilePath = commentFilePath;
        }
        return this;
    }

    /// <summary>
    /// 以当前程序集名为文件名，将编译结果 (dll / pdb / xml) 文件写入此文件夹.
    /// </summary>
    /// <remarks>
    /// 注：该逻辑 [不会] 被缓存，复用时[需重]再次调用该方法.
    /// </remarks>
    /// <param name="folder">文件夹路径,例如: "c:\myOutput"</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
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
        if (AssemblyName == string.Empty)
        {
            throw new Exception("请先指定程序集名！");
        }
        DllFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.dll");
        PdbFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.pdb");
        CommentFilePath = Path.Combine(OutputFolder, $"{AssemblyName}.xml");
        return this;
    }


    private string FileHandle(string file)
    {
        if (File.Exists(DllFilePath))
        {
            if (_cleanOutput)
            {
                File.Delete(DllFilePath);
            }
            else
            {
                return NatashaFileRepeateHelper.GetAvaliableFilePath(DllFilePath);
            }
        }
        return file;
    }
    #endregion

}



