using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        public static int RemoveByCondition<T>(this IList<T> list, Func<T, bool> condition)
        {
            var itemsToRemove = list.Where(condition).ToArray();
            var result = itemsToRemove.Length;
            foreach (var item in itemsToRemove)
            {
                list.Remove(item);
            }
            return result;
        }
    }
}
