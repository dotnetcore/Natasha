using Natasha.DynamicLoad.Base;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public class NatashaCompilerDomain : NatashaDomain, INatashaDynamicLoadContextBase
{
   
    public NatashaCompilerDomain() : base()
    {

    }
    public NatashaCompilerDomain(string key) : base(key)
    {

    }


    public override void Dispose()
    {
        _caller = null;
        base.Dispose();
    }


    private object? _caller;
    public object? GetCallerReference()
    {
        return _caller;
    }

    public void SetCallerReference(object instance)
    {
        _caller = instance;
    }
}
