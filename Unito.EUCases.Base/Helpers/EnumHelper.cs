using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class EnumHelper
    {
        public static T? Parse<T>(string value)
            where T : struct
        {
            var enumType = typeof(T);
            try
            {
                var result = (T) Enum.Parse(enumType, value);
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<EnumT> GetValues<EnumT>()
        {
            return Enum.GetValues(typeof(EnumT)).Cast<EnumT>();
        }


        public static object[] ToEnumObjArray<EnumT>(this string commaSeparatedList)
        {
            return commaSeparatedList
                .Split(',')
                .Select(p => Enum.Parse(typeof(EnumT), p))
                .ToArray();
        }

        public static EnumT[] ToEnumArray<EnumT>(this string commaSeparatedList)
        {
            if (string.IsNullOrEmpty(commaSeparatedList))
                return null;
            return commaSeparatedList
                .Split(',')
                .Select(p => (EnumT)Enum.Parse(typeof(EnumT), p))
                .ToArray();
        }

        public static string ToJoinedString<EnumT>(this EnumT[] array)
        {
            if (array.IsNullOrEmpty())
                return null;
            else
                return String.Join(",", array.Select(p => p.ToString()).ToArray());
        }

        public static EnumT ToEnum<EnumT>(this string value)
        {
            return (EnumT)Enum.Parse(typeof(EnumT), value);
        }

        public static EnumT? ToNullableEnum<EnumT>(this string value)
            where EnumT : struct
        {
            if (string.IsNullOrEmpty(value)) return null;
            return (EnumT)Enum.Parse(typeof(EnumT), value);
        }

        public static EnumT ToEnum<EnumT>(this string value, EnumT defaultValue)
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            return (EnumT)Enum.Parse(typeof(EnumT), value);
        }
    }
}
