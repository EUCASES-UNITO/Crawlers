using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unito.Eucases.Crawlers;

namespace Unito.EUCases.Crawlers.CorteCostituzionale
{
    public class CrawlerImpl:ICrawler<ParametersCorteCostituzionale,string>
    {
        static ILog _log = LogManager.GetLogger(typeof(CrawlerImpl));

        public ParametersCorteCostituzionale Parameters
        { get; set; }

        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            List<IDownloadItem> listDownloadItem = ExecuteSearchOnPronunce(Parameters.URLPronunce, Parameters.StartYear, Parameters.EndYear);            
            listDownloadItem.AddRange(ExecuteSearchOnMassime(Parameters.URLMassime, Parameters.StartYear, Parameters.EndYear));            
            IEnumerable<IDownloadItem> arrayDownloadItem = listDownloadItem;            
            return arrayDownloadItem;  
        }

        private static List<IDownloadItem> ExecuteSearch(string url, int fromYear, int toYear
                                            , string parametersGetList, string regExfPatFindNumber, string parametersSetSearchType, string regExfPatFindPronunciaLink
                                            , string postValueTypeOfAction, string postValueTypeOfAction2, string itemType)
            
        {
            var returnList = new List<IDownloadItem>();
            string parameterGetListCurrent;
            for (int i = fromYear; i <= toYear; i++)
            {
                parameterGetListCurrent = string.Format(parametersGetList, i.ToString());
                int paginaMax = 1;
                string[] myCookies;            

                string HtmlResult = string.Empty;                
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    HtmlResult = wc.UploadString(url, parameterGetListCurrent);
                    myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
                }
                // devo prendermi il numero di documenti a disposizione e dividerlo per 30 ottenendo le pagine da ispezionare                
                Match m;
                m = Regex.Match(HtmlResult, regExfPatFindNumber,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
            
                if (m.Success) // ho trovato qualcosa
                {
                    int numMax = Convert.ToInt16(m.Groups[1].Value);
                    paginaMax = numMax / 30;
                    int num = 0;
                    Math.DivRem(numMax, 30, out num);
                    if (num > 0)
                    {
                        paginaMax++;
                    }
                }

                string parametersSearchTypeCurrent;
                for (int k = 1; k <= paginaMax; k++)
                {
                    // per avanzamento pagina

                    parametersSearchTypeCurrent = string.Format(parametersSetSearchType, k);

                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.Cookie] = myCookies[0].ToString().Replace("Path=/", string.Empty);
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        HtmlResult = wc.UploadString(url, parametersSearchTypeCurrent);

                        // adesso parsifico i vari url a disposizione
                    

                        foreach (var match in Regex
                            .Matches(HtmlResult, regExfPatFindPronunciaLink, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                            .Cast<Match>())
                        {
                            //operazione=view_pronuncia&=20&pagina_pronuncia=571
                            var item = new DownloadItem();
                            item.PostValues = new List<KeyValuePair<String, String>>();
                            item.URL = url;
                            KeyValuePair<String, String> myPostValue;
                            myPostValue = new KeyValuePair<string, string>("operazione", postValueTypeOfAction);
                            item.PostValues.Add(myPostValue);
                            myPostValue = new KeyValuePair<string, string>("pagina", k.ToString());
                            item.PostValues.Add(myPostValue);

                            int fineNumero = match.Groups[1].Value.ToString().IndexOf("'");
                            int inizioId = match.Groups[1].Value.ToString().IndexOf(">");
                            string numeroPronuncia = match.Groups[1].Value.ToString().Substring(0, fineNumero);

                            myPostValue = new KeyValuePair<string, string>(postValueTypeOfAction2, numeroPronuncia);
                            item.PostValues.Add(myPostValue);
                            myPostValue = new KeyValuePair<string, string>("Cookies", myCookies[0].ToString().Replace("Path=/", string.Empty));
                            item.PostValues.Add(myPostValue);
                            string idPronuncia = match.Groups[1].Value.ToString().Substring(inizioId + 1);
                            item.Id = itemType + idPronuncia.Replace("/", "-");
                            returnList.Add(item);
                        }// fine ciclo dei singoli link
                    }// fine Using per la connessione            
                }// fine ciclo delle pagine            
            } // fine ciclo anni
            return returnList;
        }


        private static List<IDownloadItem> ExecuteSearchOnPronunce(string url, int fromYear, int toYear)
        {
            // Parametri di esempio
            //url = @"http://www.cortecostituzionale.it/actionPronuncia.do";
            //fromYear = 2000;
            //toYear = 2000;                        
            var returnList = new List<IDownloadItem>();                                        
            string HtmlResult= string.Empty;
            string parametersGetList = @"operazione=ricerca&tipo_parametro=&anno={0}&numero=&presidente=&relatore=&tipo_giudizio=&tipo_pronuncia=&testo=&dispositivo=&nome_comune=&legge_norma=&data_norma=&numero_norma=&articolo_norma=&legge_parametro=&data_parametro=&numero_parametro=&articolo_parametro=&comma_parametro=";
            string regExfPatFindNumber = @"<td class=""numeri"" align=""left"">PRONUNCE INDIVIDUATE:(.*)<";
            string parametersSetSearchType = @"operazione=scorrimento&pagina={0}&pagina_pronuncia=1";
            string regExfPatFindPronunciaLink = @"<a class=""VociLiv3off"" href=""javascript:viewPronuncia\('(.*)</a>";
            string postValueTypeOfAction = "view_pronuncia";
            string postValueTypeOfAction2 = "pagina_pronuncia";
            string itemType = "Pronuncia-";
            returnList = ExecuteSearch(url,fromYear,toYear, parametersGetList, regExfPatFindNumber, parametersSetSearchType, regExfPatFindPronunciaLink, postValueTypeOfAction,postValueTypeOfAction2,itemType);
                                                                                                                                                                                                                                     
            return returnList;
        }


        private static List<IDownloadItem> ExecuteSearchOnMassime(string url, int fromYear, int toYear)
        { 
                                
            var returnList = new List<IDownloadItem>();                            
            string parametersGetList = @"operazione=ricerca&anno={0}&numero=&tipo_pronuncia=&tipo_giudizio=&presidente=&relatore=&titolo=&testo=";                
            string regExfPatFindNumber = @"<td class=""numeri"" align=""left"">MASSIME INDIVIDUATE:(.*)<";
            string parametersSetSearchType = @"operazione=scorrimento&pagina={0}&pagina_pronuncia=1";
            string regExfPatFindPronunciaLink = @"<a class=""VociLiv3off"" href=""javascript:viewMassima\('(.*)</a>";                                                        
            string postValueTypeOfAction = "view_massima";
            string postValueTypeOfAction2 = "numero_massima";
            string itemType = "Massima-";
            returnList = ExecuteSearch(url,fromYear,toYear,parametersGetList, regExfPatFindNumber, parametersSetSearchType, regExfPatFindPronunciaLink, postValueTypeOfAction, postValueTypeOfAction2, itemType);                                                                                                         
            return returnList;
        }

        public IDownloadResult<string> Download(IDownloadItem item)
        {
            var result = new DownloadResultBase<string>
            {
                Request = item
            };


            using (WebClient wc = new WebClient())
            {
                
                wc.Headers[HttpRequestHeader.Cookie] = item.PostValues[3].Value;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string myParameters = string.Empty;
                myParameters += string.Concat(item.PostValues[0].Key, @"=", item.PostValues[0].Value, @"&");
                myParameters += string.Concat(item.PostValues[1].Key, @"=", item.PostValues[1].Value, @"&");
                myParameters += string.Concat(item.PostValues[2].Key, @"=", item.PostValues[2].Value, @"&");                
                result.Content = wc.UploadString(item.URL, myParameters);
            }

            return result;
        }
    }
}
