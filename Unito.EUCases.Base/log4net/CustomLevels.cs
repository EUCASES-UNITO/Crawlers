using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace log4net.Core
{
    public static class CustomLevels
    {

        /// <summary>
        /// Define a intermediate level between INFO and DEBUG
        /// </summary>
        public static Level Detail = new Level(35000, "DETAIL");

        public static void SetUp()
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            if ( hierarchy.LevelMap["DETAIL"] == null )
                hierarchy.LevelMap.Add(Detail);
        }
    }
}
