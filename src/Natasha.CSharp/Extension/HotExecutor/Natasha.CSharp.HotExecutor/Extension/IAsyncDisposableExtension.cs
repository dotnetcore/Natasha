public static class DisposableExtension
{
    public static void AsyncDisposedInHotExecutor(this IAsyncDisposable obj)
    {
       HEProxy.NeedBeDisposedObject(obj);
    }
    public static void AsyncDisposedInHotExecutor(this IEnumerable<IAsyncDisposable> objs)
    {
        HEProxy.NeedBeDisposedObject(objs);
    }
    public static void DisposedInHotExecutor(this IDisposable obj)
    {
        HEProxy.NeedBeDisposedObject(obj);
    }
    public static void DisposedInHotExecutor(this IEnumerable<IDisposable> objs)
    {
        HEProxy.NeedBeDisposedObject(objs);
    }
}

