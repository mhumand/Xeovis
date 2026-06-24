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
    
    /// <summary>
    /// Moves an item in a dictionary from one index position to another.
    /// Note: This creates and returns a brand new dictionary.
    /// </summary>
    public static Dictionary<TKey, TValue> MoveItemByIndex<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary, 
        int oldIndex, 
        int newIndex) where TKey : notnull
    {
        if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));
        
        if (oldIndex < 0 || oldIndex >= dictionary.Count)
            throw new ArgumentOutOfRangeException(nameof(oldIndex), "Old index is out of range.");
            
        if (newIndex < 0 || newIndex >= dictionary.Count)
            throw new ArgumentOutOfRangeException(nameof(newIndex), "New index is out of range.");

        // If the indices are the same, just return the original dictionary
        if (oldIndex == newIndex) return dictionary;

        // 1. Convert the dictionary to a List of KeyValuePairs
        var list = dictionary.ToList();

        // 2. Grab the item, remove it, and insert it at the new spot
        var item = list[oldIndex];
        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
        
        // 3. Convert back to a Dictionary (maintaining the new insertion order)
        return list.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}