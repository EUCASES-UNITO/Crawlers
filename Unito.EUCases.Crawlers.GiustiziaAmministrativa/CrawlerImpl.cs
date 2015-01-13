using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Unito.Eucases.Crawlers;

namespace Unito.EUCases.Crawlers.GiustiziaAmministrativa
{
    public class CrawlerImpl : ICrawler<ParametersGiustiziaAmministrativa, string>
    {
        static ILog _log = LogManager.GetLogger(typeof(CrawlerImpl));

        public ParametersGiustiziaAmministrativa Parameters
        { get; set; }

        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            List<IDownloadItem> myList = ExecuteSearch(Parameters.URL, Parameters.StartYear, Parameters.EndYear);
            return myList;
        }

        private static List<IDownloadItem> ExecuteSearch(string url, int fromYear, int toYear)
        {
            
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(bypassAllCertificateStuff);            
            var returnList = new List<IDownloadItem>();

            for (int i = fromYear; i <= toYear; i++)
            {
                string HtmlResult = string.Empty;
                string myParameters = @"tipoRicerca=Provvedimenti&FullText=&FullTextA=&FullTextAdvanced=&advInNotParole=&advInFrase=&ResultCount=&ordinaPer=xNumeroDocumento&xTipoDocumento=PROVVEDIMENTI&xTipoSubProvvedimento=&xTipoProvvedimento=&xSede=&xTipoProvvedimentoDecisione=XXX&xNumeroDocumento=&xAnno="
                                      + i.ToString() +
                                        "&xNProvv5=&PageNumber=&StartRow=&EndRow=&advanced=false";
                url += myParameters;

            }

            return returnList;
        }

        private static bool bypassAllCertificateStuff(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public IDownloadResult<string> Download(IDownloadItem item)
        {
            throw new NotImplementedException();
        }

        private static void singleFunction()
        {
            
            string url = @"https://94.86.40.196/cdsintra/cdsintra/AmministrazionePortale/Ricerca/index.html?tipoRicerca=Provvedimenti&FullText=&FullTextA=&FullTextAdvanced=&advInNotParole=&advInFrase=&ResultCount=&ordinaPer=xNumeroDocumento&xTipoDocumento=PROVVEDIMENTI&xTipoSubProvvedimento=&xTipoProvvedimento=&xSede=&xTipoProvvedimentoDecisione=XXX&xNumeroDocumento=200000001&xAnno=2000&xNProvv5=00001&PageNumber=&StartRow=&EndRow=&advanced=false";
            WebBrowser myWebBrowser = new WebBrowser();
            Uri myUri = new Uri(url);
            myWebBrowser.DocumentCompleted += myWebBrowser_DocumentCompleted;
            myWebBrowser.Navigate(url);
            
        }

        static void myWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser myWeb = (WebBrowser)sender;
            string myHtml = myWeb.Document.Body.OuterHtml;
        }
    }
}
