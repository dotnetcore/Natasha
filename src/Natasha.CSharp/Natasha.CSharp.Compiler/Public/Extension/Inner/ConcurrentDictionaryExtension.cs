using System.Collections.Concurrent;

static class ConcurrentDictionaryExtension
{
    public static S? Remove<T, S>(this ConcurrentDictionary<T, S?> dict, T key) where T : notnull where S : notnull
    {

        while (!dict.TryRemove(key, out var result))
        {
            if (!dict.ContainsKey(key))
            {
                return result;
            }
        }
        return default;

    }
}
