public enum UsingLoadBehavior
{
    /// <summary>
    /// 不合并 UsingCode, 只用手写的 Using.
    /// </summary>
    None = 0,
    /// <summary>
    /// 仅使用 [共享域] 加载或者编译产生的 UsingCode
    /// </summary>
    WithDefault = 1,
    /// <summary>
    /// 仅使用 [当前域] 加载或者编译产生的 UsingCode
    /// </summary>
    WithCurrent = 2,
    /// <summary>
    /// 合并 [共享域] 以及 [当前域] 的 UsingCode
    /// </summary>
    WithAll = 3
}

