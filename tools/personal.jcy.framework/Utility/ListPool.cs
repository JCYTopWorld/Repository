using System.Collections.Generic;

namespace Utility
{
    public static class ListPool<T>
    {
        private static readonly Stack<List<T>> s_ListStack = new Stack<List<T>>();

        public static List<T> Get()
        {
            return s_ListStack.Count == 0 ? new List<T>(8) : s_ListStack.Pop();
        }

        public static void Release(List<T> release)
        {
            release.Clear();
            s_ListStack.Push(release);
        }
    }
}