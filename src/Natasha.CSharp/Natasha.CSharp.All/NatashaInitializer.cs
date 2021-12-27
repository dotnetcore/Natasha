using Microsoft.CodeAnalysis;
using Natasha;
using Natasha.CSharp;
using Natasha.CSharp.Engine.SemanticAnalaysis;
using System;
using System.Collections.Generic;
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
    internal static Task Initialize(Func<string, bool>? excludeReferencesFunc = null)
    {
        if (excludeReferencesFunc == null)
        {
            excludeReferencesFunc = item => false;
        }
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        if (!_hasInitialize)
        {
            lock (_lock)
            {
                if (!_hasInitialize)
                {

                    _hasInitialize = true;

                    Task.Run(() => { var host = new AdhocWorkspace(); });
                    var task1 = Task.Run(() => {

                        var syntaxBase = new NatashaCSharpSyntax();
                        syntaxBase.AddTreeToCache("public class NatashaInitializerTest{}");

                    });
                    var task2 = Task.Run(() => {

                        NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>(excludeReferencesFunc);
                        var compilerHandler = new NatashaCSharpCompiler();
                        var option = compilerHandler.GetCompilationOptions();
                        var compiler = compilerHandler.GetCompilation(option);
                        NatashaCSharpCompiler.AddSemanticAnalysistor(UsingAnalysistor.Creator());

                    });
                    Task.WaitAll(task1, task2);
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
    public static async Task InitializeAndPreheating(Func<string, bool>? excludeReferencesFunc = null)
    {
        
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        await Initialize(excludeReferencesFunc);
        var domain = DomainManagement.Random;
        var action = NDelegate.UseDomain(domain).Action("");
        action?.Invoke();
        action?.DisposeDomain();
#if DEBUG
        stopwatch.Stop();
        Console.WriteLine($"\r\n================ [Preheating]\t预热总时长: {stopwatch.ElapsedMilliseconds}ms ================ \r\n");
#endif
  

    }
}

