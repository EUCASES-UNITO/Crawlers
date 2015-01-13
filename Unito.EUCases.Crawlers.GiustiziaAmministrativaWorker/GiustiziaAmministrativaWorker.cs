using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.CrawlersUploader;
using Unito.EUCases.Crawlers.GiustiziaAmministrativa;
using Unito.EUCases.Workers;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace Unito.EUCases.Crawlers.GiustiziaAmministrativaWorker
{
    public class GiustiziaAmministrativaWorker : WorkerBase<GiustiziaAmministrativaParameters, GiustiziaAmministativaResults>
    {
        GiustiziaAmministrativaForms _gaF;

        protected override void doWorkImplementation(System.Threading.CancellationToken token)
        {
             
            var t = new Thread(singleFunction);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            Results.Success = Results.Success ?? true;

        }

        private void singleFunction(object obj)
        {
            _gaF = new GiustiziaAmministrativaForms();
            _gaF.UrlForm = Parameters.CrawlerParameters.URL;
            _gaF.StartYear = Parameters.CrawlerParameters.StartYear;
            _gaF.EndYear = Parameters.CrawlerParameters.EndYear;
            _gaF.MaxDoc = Parameters.CrawlerParameters.MaxDocToCrawl;
            _gaF.WorkingFolder = Parameters.UploaderParameters.WorkingFolder;
            _gaF.ServiceUrl = Parameters.UploaderParameters.EUCasesServiceURL;
            _gaF.CheckIfTheFileHasToBeRenewed = Parameters.UploaderParameters.CheckIfTheFileHasToBeRenewed;
            _gaF.MaxRandomWait = Parameters.CrawlerParameters.MaxRandomWait;

            _gaF.ShowDialog();
           
        }
    }
}
