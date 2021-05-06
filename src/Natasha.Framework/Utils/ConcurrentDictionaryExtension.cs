using System.Collections.Concurrent;

public static class ConcurrentDictionaryExtension
{
    public static S Remove<T, S>(this ConcurrentDictionary<T, S> dict, T key)
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
