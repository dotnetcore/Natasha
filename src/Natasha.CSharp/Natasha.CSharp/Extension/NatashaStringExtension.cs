public static class StringExtension
{

    public static string ToReadonlyScript(this string field)
    {

        return $"Unsafe.AsRef({field})";

    }

}

