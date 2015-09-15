using System;
using System.Collections.Generic;

namespace WSDLToTypescriptProxy
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> src, Action<T> action)
        {
            foreach (var item in src)
            {
                action(item);
            }
        }

    }
}
