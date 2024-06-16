public static class DisposableExtension
{
    public static void AsyncDisposedInHotExecutor(this IAsyncDisposable obj)
    {
       ProjectDynamicProxy.NeedBeDisposedObject(obj);
    }
    public static void AsyncDisposedInHotExecutor(this IEnumerable<IAsyncDisposable> objs)
    {
        ProjectDynamicProxy.NeedBeDisposedObject(objs);
    }
    public static void DisposedInHotExecutor(this IDisposable obj)
    {
        ProjectDynamicProxy.NeedBeDisposedObject(obj);
    }
    public static void DisposedInHotExecutor(this IEnumerable<IDisposable> objs)
    {
        ProjectDynamicProxy.NeedBeDisposedObject(objs);
    }
}

