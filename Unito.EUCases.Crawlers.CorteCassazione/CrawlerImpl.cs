using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unito.Eucases.Crawlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Unito.EUCases.Crawlers.CorteCassazione
{
    public class CrawlerImpl : ICrawler<ParametersCorteCassazione, byte[]>
    {
        static ILog _log = LogManager.GetLogger(typeof(CrawlerImpl));

        ParametersCorteCassazione _parameters;        

        public ParametersCorteCassazione Parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            int numeroPagine;
            Dictionary<string, int> ListaAnni = new Dictionary<string,int>();

            string urlPost = @"http://www.italgiure.giustizia.it/sncass/isapi/hc.dll/sn.solr/sn-collection/select";            
            string urlParam =   @"facet.method=enum&facet.mincount=1&wt=json&indent=off&q=((kind:%22snciv%22))%20AND%20kind:%22snciv%22&fl=name&facet=true&facet.limit=257&facet.sort=false&facet.field=anno"; // mi da la lista di tutti i documenti per anno

            string[] myCookies; 
            List<IDownloadItem> returnItemList = new List<IDownloadItem>();

            // adesso prendo gli anni che ci sono a disposizione e leggo quanti documenti sono associati
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.UploadString(urlPost, urlParam);
                JObject o = JObject.Parse(HtmlResult);
                int counter = 0;
                foreach (var anno in o["facet_counts"]["facet_fields"]["anno"])
                {
                    counter++;
                    if(counter%2 !=0)
                    {
                        if (_parameters.Year == Convert.ToInt16(anno.ToString()))
                        ListaAnni.Add(anno.ToString(), Convert.ToInt16(anno.Next) / 10);
                    }
                }
                
            }

            foreach (var anno in ListaAnni)
            {
               int maxItems = anno.Value * 10;
               for (int k = 0; k<=maxItems; k = k+10)
               {
                   // dopo che li ho contati mi faccio restituire la lista dei documenti
                   string urlPostListOfDocument = @"http://www.italgiure.giustizia.it/sncass/isapi/hc.dll/sn.solr/sn-collection/select";
                   string urlParamListOfDocument = @"start=" + k.ToString() + "&rows=10&q=((kind%3A%22snciv%22))%20AND%20kind%3A%22snciv%22%20AND%20anno%3A%22" + anno.Key + "%22&wt=json&indent=off&sort=datdec desc,anno desc,numdec desc&fl=id%2Cfilename%2Cszdec%2Ckind%2Cssz%2Ctipoprov%2Cnumcard%2Cnumdec%2Cnumdep%2Canno%2Cdatdec%2Cpresidente%2Crelatore%2Cocr&hl=true&hl.snippets=4&hl.fragsize=100&hl.fl=ocr&hl.q=nomatch%20AND%20kind%3A%22snciv%22%20AND%20anno%3A%22" + anno.Key + "%22&hl.maxAnalyzedChars=1000000&hl.simple.pre=%3Cspan%20class%3D%22hit%22%3E&hl.simple.post=%3C%2Fspan%3E";
                   using (WebClient wc = new WebClient())
                   {
                       wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                       HtmlResult = wc.UploadString(urlPostListOfDocument, urlParamListOfDocument);
                       JObject o = JObject.Parse(HtmlResult);
                       string id = string.Empty;
                       foreach (var item in o["response"]["docs"])
                       {
                           id = item["id"].ToString();
                           foreach (var item2 in item["filename"])
                           {
                               DownloadItem myItem = new DownloadItem();
                               myItem.Id = id;
                               myItem.URL = item2.ToString().Replace(".pdf", ".clean.pdf");
                               returnItemList.Add(myItem);
                               _log.Info(item2.ToString().Replace(".pdf", ".clean.pdf"));
                           }
                       }
                   }
               }
            }                                  

            return returnItemList;
        }

        public IDownloadResult<byte[]> Download(IDownloadItem item)
        {
            var result = new DownloadResultBase<byte[]>
            {
                Request = item
            };
            // Poi li scarico uno ad uno
            string urlGetDocument = @"http://www.italgiure.giustizia.it/xway/application/nif/clean/hc.dll?verbo=attach&db=snciv&id=";            
            byte[] ByteDocument;
            
                using (WebClient wc1 = new WebClient())
                {
                    wc1.Headers[HttpRequestHeader.ContentType] = "application/pdf";
                    result.Content = wc1.DownloadData(urlGetDocument + item.URL);
                    
                }
                return result;
        }

        public string HtmlResult { get; set; }
    }
}
