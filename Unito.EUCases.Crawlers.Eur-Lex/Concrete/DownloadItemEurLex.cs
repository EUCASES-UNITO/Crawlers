using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.EurLex.Abstract;

namespace Unito.EUCases.Crawlers.EurLex.Concrete
{
    class DownloadItemEurLex: IDownloadItemEurLex
    {       

        public string URL
        {
            get;
            set;
        }

        public IList<string> NavigationPath
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public IList<KeyValuePair<string, string>> PostValues
        {
            get;
            set;
        }

        public IList<string> Tags
        {
            get;
            set;
        }
    }
}
