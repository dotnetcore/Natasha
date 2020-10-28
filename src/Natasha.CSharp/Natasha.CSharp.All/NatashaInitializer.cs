using Natasha.CSharp;
using System;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static bool _hasInitialize;
<<<<<<< HEAD
=======
    private static readonly object _lock;

    static NatashaInitializer()
    {
        NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
        NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
        NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
        _lock = new object();
    }

>>>>>>> CSharp2.0
    /// <summary>
    /// 初始化 Natasha 组件
    /// </summary>
    public static async Task Initialize()
    {

<<<<<<< HEAD
        if (!_hasInitialize)
        {
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
            _hasInitialize = true;
        }
        
=======

>>>>>>> CSharp2.0
    }

    /// <summary>
    /// 初始化 Natasha 组件并预热
    /// </summary>
    /// <returns></returns>
    public static async Task InitializeAndPreheating()
    {

        if (!_hasInitialize)
        {
<<<<<<< HEAD
            Initialize();
            var action = NDelegate.RandomDomain().Action("");
            action();
            action.DisposeDomain();
        }
        
=======
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
            
>>>>>>> CSharp2.0
    }
}

