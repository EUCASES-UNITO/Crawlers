using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class ArrayExtensions
    {
        public static bool IsNullOrEmpty(this Array source)
        {
            return source == null || source.Length == 0;
        }
    }
}
