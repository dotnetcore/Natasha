using Natasha.CSharp;
using System;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static bool _hasInitialize;
    private static readonly object _lock;

    static NatashaInitializer()
    {
        NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
        NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
        NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
        _lock = new object();
    }

    /// <summary>
    /// 初始化 Natasha 组件
    /// </summary>
    public static async Task Initialize()
    {

    }

    /// <summary>
    /// 初始化 Natasha 组件并预热
    /// </summary>
    /// <returns></returns>
    public static async Task InitializeAndPreheating()
    {

        if (!_hasInitialize)
        {
            lock (_lock)
            {
                if (!_hasInitialize)
                {
                    _hasInitialize = true;
                    var action = NDelegate.RandomDomain().Action("");
                    action();
                    action.DisposeDomain();
                }
            }
        }  
    }
}

