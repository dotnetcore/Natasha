
public static class OSStringCompare
{
    public static bool Equal(string expected, string actual)
    {
        return expected.Replace("\r\n", "\n") == actual.Replace("\r\n", "\n");

    }
}

