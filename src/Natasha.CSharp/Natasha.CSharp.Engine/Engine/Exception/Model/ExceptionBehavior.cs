namespace Natasha.Error.Model
{
    public enum ExceptionBehavior
    {
        LogAndThrow = 2,
        OnlyThrow = 4,
        Ignore = 8
    }
}
