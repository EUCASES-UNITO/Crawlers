using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.EurLex;
using Unito.EUCases.Workers;

namespace Unito.EUCases.Crawlers.EurLexWorker
{
    public class EurLexWorker : WorkerBase<EurLexParameters, EurLexResults>
    {

        protected override void doWorkImplementation(System.Threading.CancellationToken token)
        {
            
            var crawler = new CrawlerImpl(Parameters.CrawlerParameters);           

            var downloadList = crawler.GetDownloadList();
            foreach (var request in downloadList)
            {
                if (token.IsCancellationRequested)
                    break;
                try
                {
                    var result = crawler.Download(request);
                    var filePath = Path.Combine(Parameters.DestinationFolder, string.Concat(result.Request.Id, ".html"));
                    File.WriteAllText(filePath, result.Content);
                }
                catch (Exception ex)
                {
 
                }
            }
        }
    }
}
