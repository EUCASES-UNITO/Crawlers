using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.Eucases.Crawlers
{
    public class DownloadResultBase<R>:IDownloadResult<R>
    {
        public IDownloadItem Request
        {
            get; set;
        }

        public R Content
        {
            get; set;
        }
    }
}
