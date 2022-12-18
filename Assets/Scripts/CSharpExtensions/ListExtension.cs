using System.Collections.Generic;

namespace CSharpExtensions
{
    public static class ListExtension
    {
        public static void RemoveAtSwapBack<T>(this List<T> list, int index)
        {
            T temp = list[list.Count - 1];
            list[index] = temp;
            list.RemoveAt(list.Count - 1);
        }

        public static bool RemoveSwapBack<T>(this List<T> list, T item)
        {
            var index = list.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            list.RemoveAtSwapBack(index);
            return true;
        }
    }
}