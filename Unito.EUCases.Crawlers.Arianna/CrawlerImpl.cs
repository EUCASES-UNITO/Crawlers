using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unito.Eucases.Crawlers.Arianna
{
    public class CrawlerImpl : ICrawler<Parameters, string> //P parametri 
    {
        public Parameters Parameters
        {
            get;
            set;
        }


        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            var startPath = string.Format(Parameters.URL, Parameters.StartYear, Parameters.EndYear);

            String htmlCode = GetContent(startPath);

            //3) Cerco le occorrenze ovvero i path delle leggi all'interno dell'HTML:
            
            string hrefPattern = @"<a href=""(\.\.\/\.\.\/TESTO\?LAYOUT=[a-zA-Z0-9\?\&\;\.\/\=]*)""";

            var result = new List<IDownloadItem>();
            foreach (var match in Regex
                .Matches(htmlCode, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Cast<Match>())
            {
                var item = new DownloadItem
                {
                    NavigationPath = new List<string> { startPath }
                };
                var downloadUrl = match.Groups[1].Value;
                item.URL = NormalizeUrl(downloadUrl);
                

                result.Add(item);
            }

            return result;               

        }

        private string NormalizeUrl(string pDownloadUrl)
        {
            pDownloadUrl = pDownloadUrl.Replace("amp;", string.Empty);

            pDownloadUrl = pDownloadUrl.Replace("../../", string.Empty);

            return pDownloadUrl;           
        }

        private static string GetContent(string url)
        {
            using (var client = new WebClient())
            {                
                String htmlCode = client.DownloadString(url);

                client.CancelAsync();

                return htmlCode;
            }
        }

        public IDownloadResult<string> Download(Unito.Eucases.Crawlers.IDownloadItem item)
        {
            var result = new DownloadResultBase<string>
            {
                Request = item
            };

            //guardare

            item.URL = "http://arianna.consiglioregionale.piemonte.it/ariaint/" + item.URL;

            result.Content = GetContent(item.URL);

            return result;

           }
        }
    }
