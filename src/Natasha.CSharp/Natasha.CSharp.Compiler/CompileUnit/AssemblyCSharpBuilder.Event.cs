using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Immutable;
using System.Reflection;

/// <summary>
/// 程序集编译构建器 - 事件
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    /// <summary>
    /// 监听编译日志事件，默认不监听.
    /// </summary>
    /// <remarks>
    /// 注：该事件会被缓存，复用时无需重复添加方法.
    /// </remarks>
    public event Action<NatashaCompilationLog>? LogCompilationEvent;
    /// <summary>
    /// 流编译成功之后触发的事件.
    /// </summary>
    /// <remarks>
    /// 此时已编译结束，程序集已经生成并加载.
    /// </remarks>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件.
    /// </summary>
    /// <remarks>
    /// 此时已经编译结束, 但是编译失败.
    /// </remarks>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;
}

