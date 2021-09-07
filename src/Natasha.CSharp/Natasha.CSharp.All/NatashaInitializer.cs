using Natasha.CSharp;
using Natasha.CSharp.Engine.SemanticAnalaysis;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public static class NatashaInitializer
{
    private static bool _hasInitialize;
    private static readonly object _lock;

    static NatashaInitializer()
    {
        
        _lock = new object();
    }

    /// <summary>
    /// 初始化 Natasha 组件
    /// </summary>
    public static Task Initialize(bool initializeReference = true)
    {
#if DEBUG
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        if (!_hasInitialize)
        {
            lock (_lock)
            {
                if (!_hasInitialize)
                {

                    _hasInitialize = true;
                    NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>(initializeReference);
                    NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
                    NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
                    NatashaCSharpCompiler.AddSemanticAnalysistor(UsingAnalysistor.Creator());
                }
            }
            
        }
#if DEBUG
        stopwatch.Stop();
        Console.WriteLine($"\r\n================ [Regist] \t注册组件总时长: {stopwatch.ElapsedMilliseconds}ms ================ \r\n");
#endif
        return Task.CompletedTask;
    }

    /// <summary>
    /// 初始化 Natasha 组件并预热
    /// </summary>
    /// <returns></returns>
    public static async Task InitializeAndPreheating(bool initializeReference = true)
    {

        await Initialize(initializeReference);
        var domain = DomainManagement.Random;
        if (!initializeReference)
        {
            domain.AddReferencesFromDllFile(typeof(object).Assembly.Location);
        }
#if DEBUG
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
#endif
        var action = NDelegate.UseDomain(domain).Action("");
#if DEBUG
        stopwatch.Stop();
        Console.WriteLine($"\r\n================ [Preheating]\t预热总时长: {stopwatch.ElapsedMilliseconds}ms ================ \r\n");
#endif
        action();
        action.DisposeDomain();     

    }
}

