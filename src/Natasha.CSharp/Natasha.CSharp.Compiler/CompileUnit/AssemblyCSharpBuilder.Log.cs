using System;

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
    /// 和使用 <see cref="LogCompilationEvent"/> += log =>{} 一样.
    /// </summary>
    /// <remarks>
    /// 注：该事件会被缓存，复用时无需重复添加方法.
    /// </remarks>
    /// <param name="logAction"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder SetLogEvent(Action<NatashaCompilationLog> logAction)
    {
        LogCompilationEvent = logAction;
        return this;
    }

}

