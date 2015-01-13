using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.Eucases.Crawlers;

namespace Unito.EUCases.Crawlers.EurLex.Abstract
{
    interface IDownloadItemEurLex : IDownloadItem
    {
         IList<string> Tags {get;set;}
    }
}
