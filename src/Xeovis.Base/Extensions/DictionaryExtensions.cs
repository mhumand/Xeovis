namespace Xeovis.Base;

public static class DictionaryExtensions
{
    /// <summary>
    /// Converts a dictionary to a single-line comma-separated string.
    /// Example output: "Key1: Value1, Key2: Value2"
    /// </summary>
    public static string ToStringInline<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null || !dictionary.Any())
            return string.Empty;

        return string.Join(", ", dictionary.Select(kv => $"{kv.Key}: {kv.Value}"));
    }
    
    /// <summary>
    /// Converts a dictionary to a multi-line string, perfect for logging or UI display.
    /// Example output: 
    /// Key1: Value1
    /// Key2: Value2
    /// </summary>
    public static string ToStringMultiline<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null || !dictionary.Any())
            return string.Empty;

        return string.Join(Environment.NewLine, dictionary.Select(kv => $"{kv.Key}: {kv.Value}"));
    }
}