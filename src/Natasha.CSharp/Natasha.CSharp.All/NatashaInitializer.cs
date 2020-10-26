using Natasha.CSharp;
using System;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static bool _hasInitialize;
    /// <summary>
    /// 初始化 Natasha 组件
    /// </summary>
    public static async Task Initialize()
    {

        if (!_hasInitialize)
        {
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
            _hasInitialize = true;
        }
        
    }

    /// <summary>
    /// 初始化 Natasha 组件并预热
    /// </summary>
    /// <returns></returns>
    public static async Task InitializeAndPreheating()
    {

        if (!_hasInitialize)
        {
            Initialize();
            var action = NDelegate.RandomDomain().Action("");
            action();
            action.DisposeDomain();
        }
        
    }
}

