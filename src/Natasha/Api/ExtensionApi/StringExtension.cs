public static class StringExtension
{

    public static string ReadOnlyScript(this string field)
    {

        return $"Unsafe.AsRef({field})";

    }

}

