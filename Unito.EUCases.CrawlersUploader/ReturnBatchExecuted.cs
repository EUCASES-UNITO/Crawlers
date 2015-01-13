using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.CrawlersUploader
{
    public class ReturnBatchExecuted
    {
        public byte[] ZippedFile { get; set; }
        public string nameZippedFile { get; set; }
        public string XMLDocumentGroup { get; set; }
        public string nameXMLDocumentGroupFile { get; set; }
    }
}
