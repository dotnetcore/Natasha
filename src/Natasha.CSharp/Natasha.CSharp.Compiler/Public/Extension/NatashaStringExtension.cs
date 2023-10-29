public static class NatashaStringExtension
{

    public static string ToReadonlyScript(this string field)
    {

        return $"Unsafe.AsRef({field})";

    }

}

