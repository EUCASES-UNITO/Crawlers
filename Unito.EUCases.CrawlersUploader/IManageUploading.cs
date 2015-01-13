using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.CrawlersUploader
{
    public interface IManageUploading
    {
        Boolean UploadFile(String content, String IdDocument, String MD5, string url, documentgroupFormat format);
    }
}
