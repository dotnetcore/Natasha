public static class DisposableExtension
{
    public static void AsyncToHotExecutor(this IAsyncDisposable obj)
    {
       HEProxy.NeedBeDisposedObject(obj);
    }
    public static void AsyncInHotExecutor(this IEnumerable<IAsyncDisposable> objs)
    {
        HEProxy.NeedBeDisposedObject(objs);
    }
    public static void ToHotExecutor(this IDisposable obj)
    {
        HEProxy.NeedBeDisposedObject(obj);
    }
    public static void ToHotExecutor(this IEnumerable<IDisposable> objs)
    {
        HEProxy.NeedBeDisposedObject(objs);
    }
    public static void ToHotExecutor(this IEnumerable<CancellationTokenSource> objs)
    {
        HEProxy.NeedBeCancelObject(objs);
    }
    public static void ToHotExecutor(this CancellationTokenSource obj)
    {
        HEProxy.NeedBeCancelObject(obj);
    }
}

