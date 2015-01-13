using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.CorteCassazione;
using Unito.EUCases.CrawlersUploader;
using Unito.EUCases.CrawlersUploader.DAL;
using Unito.EUCases.Workers;

namespace Unito.EUCases.Crawlers.CorteCassazioneWorker
{
    public class CorteCassazioneWorker:WorkerBase<CorteCassazioneParameters,CorteCassazioneResult>
    {
        protected override void doWorkImplementation(System.Threading.CancellationToken token)
        {
            ManageUpload CrawlerUploader = new ManageUpload("CorteCassazione", "Italian", Parameters.UploaderParameters.WorkingFolder);
            IStoreMetadata _storeMetadata = new StoreMetadaOnFile(Parameters.UploaderParameters.WorkingFolder + "\\database"); 

            var crawler = new CrawlerImpl();
            crawler.Parameters = Parameters.CrawlerParameters;
            foreach (var request in crawler.GetDownloadList())
            {
                
                var result = crawler.Download(request);
                var filePath = Path.Combine(Parameters.DestinationFolder, string.Concat(result.Request.Id, ".pdf"));

                System.IO.FileStream _FileStream =
                                    new System.IO.FileStream(filePath, System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write);
                _FileStream.Write(result.Content, 0, result.Content.Length);
                _FileStream.Close();
                


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
                CrawlerUploader.PrepareToSendData(); //"test"
                CrawlerUploader.UploadFileBinary(result.Content, result.Request.Id, sbHash.ToString(), result.Request.URL, documentgroupFormat.texthtml);
                CrawlerUploader.UrlService = Parameters.UploaderParameters.EUCasesServiceURL;
                CrawlerUploader.UploadAllFilesToService();

                Results.DownloadedDocs++;
            }
        }
    }
}
