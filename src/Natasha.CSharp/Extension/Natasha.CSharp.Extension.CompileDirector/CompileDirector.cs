using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public class CompileDirector
{
    private readonly ConcurrentDictionary<string, int> _usingCounter;
    private Action<AssemblyCSharpBuilder>? _configBuilder;
    private readonly NatashaUsingCache _usingCache;
    private readonly int _minCounter;
    private readonly object _lockObj = new();
    /// <summary>
    /// 编译成功后 using 最低的采用记录数。大于这个次数，将会采用这个 using 进行编译。
    /// </summary>
    /// <param name="minCounter"></param>
    public CompileDirector(int minCounter = 2)
    {
        _usingCounter = [];
        _minCounter = minCounter;
        _usingCache = new();
    }
    /// <summary>
    /// 传入 builder 的配置逻辑，builder 在创建时执行该逻辑。
    /// </summary>
    /// <param name="configAction"></param>
    /// <returns></returns>
    public CompileDirector ConfigBuilder(Action<AssemblyCSharpBuilder> configAction)
    {
        _configBuilder = configAction;
        return this;
    }

    private UsingLoadBehavior _usingCombineStrategy = UsingLoadBehavior.WithAll;
    /// <summary>
    /// 配置编译出错后，重建脚本时的 using 合并策略。
    /// </summary>
    /// <param name="usingLoadBehavior"></param>
    /// <returns></returns>
    public CompileDirector ConfigUsingConverSrategy(UsingLoadBehavior usingLoadBehavior)
    {
        _usingCombineStrategy = usingLoadBehavior;
        return this;
    }

    /// <summary>
    /// 从编译导演这里创建一个属于该场景的编译单元。
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder CreateBuilder()
    {
        var builder = new AssemblyCSharpBuilder();
        _configBuilder?.Invoke(builder);
        builder.ConfigScriptHandle(script => _usingCache.ToString() + script);
        builder.CompileSucceedEvent += Builder_CompileSucceedEvent;
        builder.CompileFailedEvent += (compilation, diagnostic) =>
        {
            List<string> tempTree = [];
            builder.WithCombineUsingCode(_usingCombineStrategy);
            foreach (var item in builder.SyntaxTrees)
            {
                tempTree.Add(item.ToString());
            }
            builder.SyntaxTrees.Clear();
            builder.WithSemanticCheck();
            foreach (var item in tempTree)
            {
                builder.Add(item);
            }

        };
        return builder;
    }

    /// <summary>
    /// 通过编译导演获取该场景下的程序集，编译导演将学习 using 的使用情况，以便下次编译时使用。
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public Assembly GetAssembly(AssemblyCSharpBuilder builder)
    {
        try
        {
            return builder.GetAssembly();
        }
        catch
        {
#if DEBUG
            Console.WriteLine("触发重组编译！");
#endif
            return builder.GetAssembly();
        }
    }
    private void Builder_CompileSucceedEvent(Microsoft.CodeAnalysis.CSharp.CSharpCompilation arg1, System.Reflection.Assembly arg2)
    {
        foreach (var tree in arg1.SyntaxTrees)
        {
            AnalyzeSyntaxTree(tree);
        }

        foreach (var item in _usingCounter)
        {
            if (item.Value > _minCounter)
            {
                if (!_usingCache.HasUsing(item.Key))
                {
#if DEBUG
                    Console.WriteLine($"本次被录用的命名空间为：{item.Key}");
#endif
                    _usingCache.Using(item.Key);
                }
            }
        }
    }
    private void AnalyzeSyntaxTree(SyntaxTree tree)
    {
        var root = tree.GetRoot();

        var usingStatements = root.DescendantNodes().OfType<UsingDirectiveSyntax>();

        foreach (var usingStatement in usingStatements)
        {
            AddUsing(usingStatement!.Name!.ToFullString());
        }
    }
    private void AddUsing(string usingName)
    {
        lock (_lockObj)
        {
            if (_usingCounter.TryGetValue(usingName, out int value))
            {
                if (value != int.MaxValue)
                {
#if DEBUG
                    Console.WriteLine($"{usingName} 命名空间使用已达到 {value + 1} 次！");
#endif
                    _usingCounter[usingName] = value + 1;
                }
            }
            else
            {
#if DEBUG
                Console.WriteLine($"本次学习到了 {usingName} 命名空间。");
#endif
                _usingCounter[usingName] = 0;
            }
        }
    }

    /// <summary>
    /// 重建学习到的 using 缓存
    /// </summary>
    public void RebuildUsingCache()
    {
        _usingCache.Dispose();
        lock (_lockObj)
        {
            foreach (var item in _usingCounter)
            {
                if (item.Value > _minCounter)
                {
                    _usingCache.Using(item.Key);
                }
            }
        }
        
    }
}

