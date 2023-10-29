#if !NET6_0_OR_GREATER

namespace System.Reflection
{
    public enum NullabilityState
    {
        //
        // 摘要:
        //     Nullability context not enabled (oblivious).
        Unknown = 0,
        //
        // 摘要:
        //     Non-nullable value or reference type.
        NotNull = 1,
        //
        // 摘要:
        //     Nullable value or reference type.
        Nullable = 2
    }
}
#endif
