using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Workers;

namespace Unito.EUCases.Crawlers.NormattivaWorker
{
    public class NormattivaResults:ResultBase
    {
        private int _downloadedDocs;
        public int DownloadedDocs
        {
            get { return _downloadedDocs; }
            set { _downloadedDocs = value; OnPropertyChanged("DownloadedDocs");  }
        }
        
        
    }
}
