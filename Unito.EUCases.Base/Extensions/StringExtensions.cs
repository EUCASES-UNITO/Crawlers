using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {
            return String.IsNullOrEmpty(source);
        }

        public static string RemoveDoubleSpaces(this string source)
        {
            if (source.IsNullOrEmpty())
                return source;
            var result = source;
            while (result.Contains("  "))
                result = result.Replace("  ", " ");
            return result;
        }
    }
}
