
public static class NameGenerator
{
    public static string GetRandomName()
    {
        return "T" + Guid.NewGuid().ToString("N");
    }
}

