using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Workers;
using Unito.EUCases.Crawlers.CorteCostituzionale;
using System.IO;
using Unito.EUCases.CrawlersUploader;
using System.Security.Cryptography;
using Unito.EUCases.CrawlersUploader.DAL;
using System.Diagnostics;
using Unito.EUCases.Email;


namespace Unito.EUCases.Crawlers.CorteCostituzionaleWorker
{
    public class CorteCostituzionaleWorker:WorkerBase<CorteCostituzionaleParameters,CorteCostituzionaleResults>
    {
        

        protected override void doWorkImplementation(System.Threading.CancellationToken token)
        {
            ManageUpload CrawlerUploader = new ManageUpload("CorteCostituzionale","Italian",Parameters.UploaderParameters.WorkingFolder);
            IStoreMetadata _storeMetadata = new StoreMetadaOnFile(Parameters.UploaderParameters.WorkingFolder + "\\database"); 

            var crawler = new CrawlerImpl();
            crawler.Parameters = Parameters.CrawlerParameters;
            var downloadList = crawler.GetDownloadList();
            foreach (var request in downloadList)
            {
                if (token.IsCancellationRequested)
                    break;
                try
                {

                    bool HaveToBeDownloaded = Parameters.UploaderParameters.CheckIfTheFileHasToBeRenewed;

                    if (!_storeMetadata.ExistsFile(request.Id) || HaveToBeDownloaded)
                    {                    
                        if(Parameters.CrawlerParameters.MaxRandomWait !=0)
                        { 
                            Random rnd = new Random();
                            int seconds = rnd.Next(1, Parameters.CrawlerParameters.MaxRandomWait) * 1000;
                            System.Threading.Thread.Sleep(seconds);
                        }
                        var result = crawler.Download(request);
                        var filePath = Path.Combine(Parameters.DestinationFolder, string.Concat( result.Request.Id,".html"));
                        File.WriteAllText(filePath, result.Content);
                        var filePathTxt = Path.Combine(Parameters.DestinationFolder, string.Concat(result.Request.Id, ".txt"));
                        File.WriteAllText(filePathTxt, request.URL);
                        byte[] fileMd5;
                        using (var md5 = MD5.Create())
                        {
                            using (var stream = File.OpenRead(filePath))
                            {
                                fileMd5 = md5.ComputeHash(stream);
                            }
                        }
                        StringBuilder sbHash = new StringBuilder();
                        for (int i = 0; i < fileMd5.Length; i++)
                        {
                            sbHash.Append(fileMd5[i].ToString("x2"));
                        }
                    
                        CrawlerUploader.SetZippedFileName(result.Request.Id);
                        CrawlerUploader.PrepareToSendData();
                        CrawlerUploader.UploadFile(result.Content, result.Request.Id, sbHash.ToString(), result.Request.URL, documentgroupFormat.texthtml);
                        CrawlerUploader.UrlService = Parameters.UploaderParameters.EUCasesServiceURL;
                        CrawlerUploader.UploadAllFilesToService();

                        Results.DownloadedDocs++;
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Format("Unexpected error during download of [{0}].", request.URL);
                    _log.Error(msg, ex);
                    Results.Success = false;
                }               
            }
            Results.Success = Results.Success ?? true;

            EmailHelper sender = new EmailHelper();
            sender.SendEmail("Corte Costituzionale Worker", "Ho finito", Parameters.EmailParameters);

            if (Parameters.CrawlerParameters.CallNext != string.Empty)
            {
                Process.Start(Parameters.CrawlerParameters.CallNext);
            }
        }
    }
}
