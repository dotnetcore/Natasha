public static class IAsyncDisposableExtension
{
    public static void DisposedInHotExecutor(this IAsyncDisposable obj)
    {
       ProjectDynamicProxy.NeedBeDisposedObject(obj);
    }
    public static void DisposedInHotExecutor(this IEnumerable<IAsyncDisposable> objs)
    {
        ProjectDynamicProxy.NeedBeDisposedObject(objs);
    }
}

