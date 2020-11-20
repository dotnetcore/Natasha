using System;
/// <summary>
/// 访问级别枚举
/// </summary>
[Flags]
public enum AccessFlags
{
    None,
    Public,
    Private,
    Protected,
    Internal,
    InternalAndProtected
}

