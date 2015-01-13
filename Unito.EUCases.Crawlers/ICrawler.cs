using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Unito.Eucases.Crawlers
{
    public interface ICrawler<P, R>
    {
        P Parameters { get; set; }

        IEnumerable<IDownloadItem> GetDownloadList();

        IDownloadResult<R> Download(IDownloadItem item);
    }
}
