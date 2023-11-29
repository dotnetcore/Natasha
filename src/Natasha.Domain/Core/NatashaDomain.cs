using System;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;


[assembly:InternalsVisibleTo("NatashaFunctionUT, PublicKey=002400000480000094000000060200000024000052534131000400000100010069acb31dd0d9918441d6ed2b49cd67ae17d15fd6ded4ccd2f99b4a88df8cddacbf72d5897bb54f406b037688d99f482ff1c3088638b95364ef614f01c3f3f2a2a75889aa53286865463fb1803876056c8b98ec57f0b3cf2b1185de63d37041ba08f81ddba0dccf81efcdbdc912032e8d2b0efa21accc96206c386b574b9d9cb8")]
[assembly: InternalsVisibleTo("PluginFunctionUT, PublicKey=002400000480000094000000060200000024000052534131000400000100010069acb31dd0d9918441d6ed2b49cd67ae17d15fd6ded4ccd2f99b4a88df8cddacbf72d5897bb54f406b037688d99f482ff1c3088638b95364ef614f01c3f3f2a2a75889aa53286865463fb1803876056c8b98ec57f0b3cf2b1185de63d37041ba08f81ddba0dccf81efcdbdc912032e8d2b0efa21accc96206c386b574b9d9cb8" )]
/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain : AssemblyLoadContext, IDisposable
{
   
    protected NatashaDomain() : base("Default")
    {

        Default.Resolving += Default_Resolving;
        Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
        _assemblyLoadBehavior = AssemblyCompareInfomation.None;
        _excludePluginReferencesFunc = item => false;
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);
        Unsafe.AsRef(in DefaultDomain) = this;

    }
    public NatashaDomain(string key) : base(key, true)
    {

        if (key == "Default")
        {
            throw new Exception("不能重复创建主域!");
        }
       
        _assemblyLoadBehavior = AssemblyCompareInfomation.None;
        _excludePluginReferencesFunc = item => false;
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);

    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            
        }
    }

}
