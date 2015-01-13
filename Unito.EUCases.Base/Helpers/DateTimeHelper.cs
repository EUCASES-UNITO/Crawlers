//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System
{
    public static class DateTimeHelper
    {
        public static bool IsEndOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month) == date.Day;
        }

        public static DateTime FirstOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }


        public static DateTime FirstOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }

        public static bool IsInLeapYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year);
        }

        /// <summary>
        /// Adds the specified number of months to the value of this instance.
        /// When starting date is end of month the resulting date will be end of month 
        /// </summary>
        /// <param name="date"></param>
        /// <param name="months">A number of months. The months parameter can be negative or positive.</param>
        /// <returns> A System.DateTime whose value is the sum of the date and months.</returns>
        public static DateTime AddMonthsExtended(this DateTime date, int months)
        {
            var result = date.AddMonths(months);
            if (date.IsEndOfMonth())
                result = result.EndOfMonth();
            return result;
        }

        static DateTime _sqlDateTimeMinDate = new DateTime(1753, 1, 1);
        public static DateTime SqlDateTimeMinDate
        {
            get { return _sqlDateTimeMinDate; }
        }

        static DateTime _sqlDateTimeMaxDate = new DateTime(9999, 12, 31);
        public static DateTime SqlDateTimeMaxDate
        {
            get { return _sqlDateTimeMaxDate; }
        }

    }
}
