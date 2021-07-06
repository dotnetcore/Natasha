using System.Collections.Concurrent;

public static class ConcurrentDictionaryExtension
{
    public static S Remove<T, S>(this ConcurrentDictionary<T, S> dict, T key)
    {

        S result;
        while (!dict.TryRemove(key, out result))
        {
            if (!dict.ContainsKey(key))
            {
                return result;
            }
        }
        return result;

    }
}
