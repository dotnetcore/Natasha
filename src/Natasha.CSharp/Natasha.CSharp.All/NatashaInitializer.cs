using Natasha.CSharp;
using System;
using System.Threading.Tasks;

public class NatashaInitializer
{

    /// <summary>
    /// 初始化 Natasha 组件
    /// </summary>
    public static void Initialize()
    {
        NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
        NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
        NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
    }

    /// <summary>
    /// 初始化 Natasha 组件并预热
    /// </summary>
    /// <returns></returns>
    public static async Task InitializeAndPreheating()
    {
        Initialize();
        var action = NDelegate.RandomDomain().Action("");
        action();
        action.DisposeDomain();
    }
}

