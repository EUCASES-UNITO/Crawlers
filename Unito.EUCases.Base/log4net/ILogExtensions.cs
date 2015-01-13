using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace log4net
{
    public static class ILogExtensions
    {
        public static void Detail(this ILog log, string message, Exception ex = null)
        {
            log.Logger.Log(
                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                CustomLevels.Detail, message, ex);
        }

        public static void DetailFormat(this ILog log, string message, params object[] args)
        {
            var formattedMsg = string.Format(message, args);
            Detail(log, formattedMsg);
        }

        public static void Verbose(this ILog log, string message, Exception ex = null)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                Level.Verbose, message, ex);
        }

        public static void VerboseFormat(this ILog log, string message, params object[] args)
        {
            var formattedMsg = string.Format(message, args);
            Verbose(log, formattedMsg);
        }
    }
}
