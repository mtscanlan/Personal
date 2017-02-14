using System;
using System.Collections.Generic;

namespace ToastUtil
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T value in enumerable)
            {
                action(value);
            }
        }
    }
}
