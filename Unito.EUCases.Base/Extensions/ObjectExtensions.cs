using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsIn<T>(this T value, params T[] values)
        {
            return values.Any(_ => object.Equals(_, value));
        }
    }
}
