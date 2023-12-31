public static class NatashaStringExtension
{

    public static string ToReadonlyScript(this string field)
    {
#if NET8_0_OR_GREATER
        return $"Unsafe.AsRef(in {field})";
#else
        return $"Unsafe.AsRef({field})";
#endif
    }

}

