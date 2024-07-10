
using Natasha.CSharp.Extension.MethodCreator;

namespace System
{
    public static class NatashaUnsafeAsyncDelegateExtension
    {
        #region Action Delegate
        public static Action<T1> ToUnsafeAsyncAction<T1>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1>>();
        }
        public static Action<T1, T2> ToUnsafeAsyncAction<T1, T2>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2>>();
        }
        public static Action<T1, T2, T3> ToUnsafeAsyncAction<T1, T2, T3>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3>>();
        }
        public static Action<T1, T2, T3, T4> ToUnsafeAsyncAction<T1, T2, T3, T4>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4>>();
        }
        public static Action<T1, T2, T3, T4, T5> ToUnsafeAsyncAction<T1, T2, T3, T4, T5>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
        }
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToUnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
        }
        #endregion


        #region Func Delegate
        public static Func<T1> ToUnsafeAsyncFunc<T1>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1>>();
        }
        public static Func<T1, T2> ToUnsafeAsyncFunc<T1, T2>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2>>();
        }
        public static Func<T1, T2, T3> ToUnsafeAsyncFunc<T1, T2, T3>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3>>();
        }
        public static Func<T1, T2, T3, T4> ToUnsafeAsyncFunc<T1, T2, T3, T4>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4>>();
        }
        public static Func<T1, T2, T3, T4, T5> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>();
        }
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToUnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this NatashaSlimMethodBuilder builder)
        {
            return builder.ToUnsafeAsyncDelegate<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>();
        }
        #endregion

    }
}
