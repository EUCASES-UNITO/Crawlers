using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.Eucases.Crawlers
{
    public interface IDownloadResult<R>
    {
        IDownloadItem Request { get; set; }
        R Content { get; set; }
    }
}
