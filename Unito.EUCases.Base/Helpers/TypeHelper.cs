using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Base.Helpers
{
    public static class TypeHelper
    {
        public static A GetCustomAttribute<A>(this Type type)
            where A : Attribute
        {
            var attributeType = type.GetCustomAttributes(typeof(A), true);
            if (attributeType.Length != 1)
                return null;

            return attributeType[0] as A;
        }

        public static A[] GetCustomAttributes<A>(this Type type)
            where A : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(A), true);

            return attributes.Cast<A>().ToArray();
        }
    }
}
