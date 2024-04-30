
namespace System
{
    public static class SystemDelegateExtension
    {
        #region Action Delegate
        public static Action<T1>? ToAction<T1>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1>>();
        }
        public static Action<T1, T2>? ToAction<T1, T2>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2>>();
        }
        public static Action<T1, T2, T3>? ToAction<T1, T2, T3>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3>>();
        }
        public static Action<T1, T2, T3, T4>? ToAction<T1, T2, T3, T4>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4>>();
        }
        public static Action<T1, T2, T3, T4, T5>? ToAction<T1, T2, T3, T4, T5>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6>? ToAction<T1, T2, T3, T4, T5, T6>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7>? ToAction<T1, T2, T3, T4, T5, T6, T7>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8>? ToAction<T1, T2, T3, T4, T5, T6, T7, T8>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7, T8>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>? ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>? ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>? ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
        }
        #endregion


        #region Func Delegate
        public static Func<T1>? ToFunc<T1>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1>>();
        }
        public static Func<T1, T2>? ToFunc<T1, T2>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2>>();
        }
        public static Func<T1, T2, T3>? ToFunc<T1, T2, T3>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3>>();
        }
        public static Func<T1, T2, T3, T4>? ToFunc<T1, T2, T3, T4>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4>>();
        }
        public static Func<T1, T2, T3, T4, T5>? ToFunc<T1, T2, T3, T4, T5>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6>? ToFunc<T1, T2, T3, T4, T5, T6>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7>? ToFunc<T1, T2, T3, T4, T5, T6, T7>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8>? ToFunc<T1, T2, T3, T4, T5, T6, T7, T8>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7, T8>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>? ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>? ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>? ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(in this (string script, AssemblyCSharpBuilder builder) buildInfo)
        {
            return buildInfo.CreateMethod<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
        }
        #endregion

    }
}
