using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Concurrent;
/// <summary>
/// 程序集编译构建器 - EMIT选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{ 
    private ConcurrentQueue<Func<EmitOptions, EmitOptions>>? _emitOptionHandle;
    /// <summary>
    /// 追加对 emitOption 的处理逻辑.
    /// <list type="bullet">
    /// <item>一次性配置，不可重用.</item>
    /// <item>多次调用会进入配置队列.</item>
    /// <item>调用 <see cref="GetAssembly"/> 后清空队列.</item>
    /// <item>调用 <see cref="Clear"/> 后清空队列.</item>
    /// <item>调用 <see cref="ClearEmitOptionCache"/> 后清空队列.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：该配置属于一次性配置，若重复使用该配置逻辑，请在这次编译后重新调用该方法.
    /// </remarks>
    /// <param name="handleAndReturnNewEmitOption"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ConfigEmitOptions(Func<EmitOptions, EmitOptions> handleAndReturnNewEmitOption)
    {
        _emitOptionHandle ??= new ConcurrentQueue<Func<EmitOptions, EmitOptions>>();
        _emitOptionHandle.Enqueue(handleAndReturnNewEmitOption);
        return this;
    }
}
