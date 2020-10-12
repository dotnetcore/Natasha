public static class StringExtension
{

    public static string ReadonlyScript(this string field)
    {

        return $"Unsafe.AsRef({field})";

    }

}

