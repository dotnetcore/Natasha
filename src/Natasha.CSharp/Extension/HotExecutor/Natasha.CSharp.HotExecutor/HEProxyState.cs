
public static class HEProxyState<T>
{
    public static T Value = default!;
}

public static class HEProxyState
{
    public static void SetValue<T>(T value)
    {
        HEProxyState<T>.Value = value;
    }
}

