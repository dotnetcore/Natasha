public static class DisposableExtension
{
    public static void DisposedInHotExecutor(this IAsyncDisposable obj)
    {
       ProjectDynamicProxy.NeedBeDisposedObject(obj);
    }
    public static void DisposedInHotExecutor(this IEnumerable<IAsyncDisposable> objs)
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

