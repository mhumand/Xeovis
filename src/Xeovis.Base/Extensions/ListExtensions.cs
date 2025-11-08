namespace Xeovis.Base;

public static class ListExtensions
{
    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }
    
    public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
    {
        if (list == null) return;

        if (oldIndex < 0 || oldIndex >= list.Count) return;
        if (newIndex < 0 || newIndex >= list.Count) return;

        // Move only if indices differ
        if (oldIndex == newIndex) return;

        T item = list[oldIndex];
        list.RemoveAt(oldIndex);

        // // if we removed before the new index, the new index shifts left
        // if (newIndex > oldIndex) newIndex--;
        list.Insert(newIndex, item);
    }
}