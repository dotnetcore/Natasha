public static class StringToUnsafeMethodExtension
{
    #region Action Delegate
    public static Action<T1> ToUnsafeAction<T1>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1>>();
    }
    public static Action<T1, T2> ToUnsafeAction<T1, T2>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2>>();
    }
    public static Action<T1, T2, T3> ToUnsafeAction<T1, T2, T3>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3>>();
    }
    public static Action<T1, T2, T3, T4> ToUnsafeAction<T1, T2, T3, T4>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4>>();
    }
    public static Action<T1, T2, T3, T4, T5> ToUnsafeAction<T1, T2, T3, T4, T5>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6> ToUnsafeAction<T1, T2, T3, T4, T5, T6>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
    }
    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToUnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
    }
    #endregion


    #region Func Delegate
    public static Func<T1> ToUnsafeFunc<T1>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1>>();
    }
    public static Func<T1, T2> ToUnsafeFunc<T1, T2>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2>>();
    }
    public static Func<T1, T2, T3> ToUnsafeFunc<T1, T2, T3>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3>>();
    }
    public static Func<T1, T2, T3, T4> ToUnsafeFunc<T1, T2, T3, T4>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4>>();
    }
    public static Func<T1, T2, T3, T4, T5> ToUnsafeFunc<T1, T2, T3, T4, T5>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6> ToUnsafeFunc<T1, T2, T3, T4, T5, T6>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
    }
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToUnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this string script)
    {
        return script.WithSmartMethodBuilder().ToUnsafeDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
    }
    #endregion
}
