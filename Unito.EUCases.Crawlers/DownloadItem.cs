using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.Eucases.Crawlers
{
    public class DownloadItem:IDownloadItem
    {
        public string URL {get; set;}

        public IList<string> NavigationPath { get; set;}

        public string Id { get; set;}

        public IList<KeyValuePair<String, String>> PostValues
        { get; set; }

    }
}
