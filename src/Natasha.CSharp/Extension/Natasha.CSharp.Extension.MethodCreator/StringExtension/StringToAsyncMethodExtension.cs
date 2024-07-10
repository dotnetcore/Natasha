public static class StringToAsyncMethodExtension
{
    #region Action Delegate
    public static Action<T1> ToAsyncAction<T1>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1>>();
    }
    public static Action<T1, T2> ToAsyncAction<T1, T2>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2>>();
    }
    public static Action<T1, T2, T3> ToAsyncAction<T1, T2, T3>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3>>();
    }
    public static Action<T1, T2, T3, T4> ToAsyncAction<T1, T2, T3, T4>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4>>();
    }
    public static Action<T1, T2, T3, T4, T5> ToAsyncAction<T1, T2, T3, T4, T5>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6> ToAsyncAction<T1, T2, T3, T4, T5, T6>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
    }
    #endregion


    #region Func Delegate
    public static Func<T1> ToAsyncFunc<T1>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1>>();
    }
    public static Func<T1, T2> ToAsyncFunc<T1, T2>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2>>();
    }
    public static Func<T1, T2, T3> ToAsyncFunc<T1, T2, T3>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3>>();
    }
    public static Func<T1, T2, T3, T4> ToAsyncFunc<T1, T2, T3, T4>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4>>();
    }
    public static Func<T1, T2, T3, T4, T5> ToAsyncFunc<T1, T2, T3, T4, T5>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6> ToAsyncFunc<T1, T2, T3, T4, T5, T6>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this string script)
    {
        return script.WithSmartMethodBuilder().ToAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
    }
    #endregion
}
