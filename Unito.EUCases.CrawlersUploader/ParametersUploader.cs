using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.CrawlersUploader
{
    public class ParametersUploader
    {
        public string EUCasesServiceURL { get; set; }
        public string WorkingFolder { get; set; }
        public bool CheckIfTheFileHasToBeRenewed { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
