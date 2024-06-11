
public static class ProxyState<T>
{
    public static T Value = default!;
}

public static class ProxyState
{
    public static void SetValue<T>(T value)
    {
        ProxyState<T>.Value = value;
    }
}

