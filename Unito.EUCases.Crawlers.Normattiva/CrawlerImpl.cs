using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unito.Eucases.Crawlers;
namespace Unito.EUCases.Crawlers.Normattiva
{
    public class CrawlerImpl : ICrawler<Parameters, string>
    {
        #region ATTRIBUTES

        static ILog _log = LogManager.GetLogger(typeof(CrawlerImpl));

        private int count = 0;

        private int countId = 0;

        private List<string> htmlFromPagePaths = new List<string>();        

        //string startPath = "http://www.normattiva.it/do/ricerca/avanzata/aggiornamenti/0?numeroProvvedimento=&giornoProvvedimento=&meseProvvedimento=&annoProvvedimento=&numeroArticolo=&siglaProvvedimento=&tipoRicercaTitolo=ALL_WORDS&titolo=&titoloNot=&tipoRicercaTesto=ALL_WORDS&testo=&testoNot=&giornoPubblicazioneDa=&mesePubblicazioneDa=&annoPubblicazioneDa=&giornoPubblicazioneA=&mesePubblicazioneA=&annoPubblicazioneA=";

        string startPath = "http://www.normattiva.it/ricerca/avanzata/aggiornamenti";
       
        string giornoVigenza = string.Empty;

        string meseVigenza = string.Empty;

        string annoVigenza = string.Empty;

        string creaAnteprimaStampa = string.Empty;

        string dataPubblicazioneGazzetta = string.Empty;

        string codiceRedazionale = string.Empty;

        private string[] cookies; //21 mag 2014

        List<string> articoliSelectedList = new List<string>();

        List<IDownloadItem> resultFin = new List<IDownloadItem>();        
       
        #endregion

        /// <summary>
        /// Contiene tutti i parametri passati dalla form del worker
        /// </summary>
        /// 

        public Parameters Parameters { get; set; }

        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            _log.Info("Start getting download list...");

            //1) ATTERRO SULLA PAGINA DI RICERCA ED EFFETTUO LA RICERCA CON I PARAMETRI CHE RECUPERO DA Parameters

            String searchPageHtmlCode = GetContent(startPath);

            cookies = GetCookies(startPath); 

            cookies[0] = cookies[0].Replace("; Path=/", string.Empty);

            cookies[1] = cookies[1].Replace("; Path=/", string.Empty);

            int endMonth = Parameters.EndMonth;

            int parametroMeseA = endMonth;

            int endYear = Parameters.EndYear;

            int parametroAnnoA = endYear;

            int startYear = Parameters.StartYear;

            int parametroAnnoDA = startYear;

            if (Parameters.EndYear > Parameters.StartYear)
	        {
		        Parameters.EndYear = Parameters.StartYear;
            }

            var result = new List<IDownloadItem>();

            while (Parameters.EndYear != parametroAnnoA || Parameters.StartMonth != parametroMeseA + 1)
            {
                Parameters.EndMonth = Parameters.StartMonth;

                htmlFromPagePaths.Clear();               

                var myList = new List<string>();

                string htmlCode = string.Empty;

                NameValueCollection formData = CreatesFullParameterizedUrl(Parameters.StartMonth.ToString(), Parameters.StartYear.ToString(), Parameters.EndMonth.ToString(), Parameters.EndYear.ToString());

                using (var client = new WebClient())
                {
                    string requestCookie = string.Concat(cookies[1].ToString(), "; ");//inverto l'ordine dei parametri cookie per adeguarli all'ordine con cui vengono inviati nelle request successive alla prima

                    requestCookie = string.Concat(requestCookie, cookies[0].ToString());
                    
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                    client.Headers["Cookie"] = requestCookie;

                    byte[] responseBytes = client.UploadValues("http://www.normattiva.it/do/ricerca/avanzata/aggiornamenti/0", "POST", formData);

                    htmlCode = Encoding.ASCII.GetString(responseBytes);

                    client.CancelAsync();
                }

                string hrefPattern = @"<a href=""/do/ricerca/avanzata/aggiornamenti/";

                //2) CARICO TUTTE LE PAGINE DEI RISULTATI NELL'OGGETTO htmlFromPagePaths 
            
                htmlFromPagePaths = GetHtmlList(htmlCode, hrefPattern, cookies);

                string singleDocumentHrefPattern = @"<p class=""special"">\s*<a href=""(.*)"">";

                 //3) DA OGNI item DI htmlFromPagePaths RECUPERO I PATH DEI SINGOLI DOCUMENTI E NE CREO UN ITEM IN RESULT

                int countP = htmlFromPagePaths.Count();

                foreach (var htmlItem in htmlFromPagePaths) 
                {
                    hrefPattern = CreatesIDownloadItemList(cookies, hrefPattern, singleDocumentHrefPattern, result, countP, htmlItem);

                    countP--;  
                }

                foreach (var itemResult in result)
	            {
                    resultFin.Add(itemResult);                    
	            }

                if (Parameters.StartMonth < 12)
                {                   
                    Parameters.StartMonth = Parameters.StartMonth + 1;
                }
                else 
                {
                    Parameters.StartMonth = 1;
                    
                    Parameters.StartYear++;

                    Parameters.EndYear = Parameters.StartYear;                  
                }

                result.Clear();                

                countP = 0;               
            }

            return resultFin;
        }            
     
        public IDownloadResult<string> Download(IDownloadItem item)
        {
            var result = new DownloadResultBase<string>
            {
                Request = item
            };

            result.Content = GetContent(item); 

            return result;
        }

        #region METHODS NOT INTERFACE      

        private string CreatesIDownloadItemList(string[] cookies, string hrefPattern, string singleDocumentHrefPattern, List<IDownloadItem> result, int count, string htmlItem) 
        {
           foreach (var match in Regex
                .Matches(htmlItem, singleDocumentHrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Cast<Match>())
            {
                hrefPattern = hrefPattern.Replace(@"<a href=""", string.Empty); //tiene conto della pagina risultati della ricerca

                string pathSearchPageNumber = string.Concat("http://www.normattiva.it", hrefPattern); //tiene conto della pagina risultati della ricerca

                pathSearchPageNumber = string.Concat(pathSearchPageNumber, count.ToString()); //tiene conto della pagina risultati della ricerca

                string singleDocumentHrefPatternNew = singleDocumentHrefPattern; 

                singleDocumentHrefPatternNew = match.ToString().Replace("<a href=", string.Empty); //tiene conto della pagina risultati della ricerca

                string singleDocumentPath1 = string.Concat("http://www.normattiva.it", match.Groups[1].Value); //tiene conto della pagina risultati della ricerca

                string singleDocumentPath2 = singleDocumentPath1.Replace("caricaDettaglioAtto", "vediMenuHTML"); //tiene conto della pagina risultati della ricerca

                singleDocumentPath2 = string.Concat(singleDocumentPath2, "&currentSearch=ricerca_avanzata_aggiornamenti"); //tiene conto della pagina risultati della ricerca

                var item = new DownloadItem
                {
                    NavigationPath = new List<string> { startPath, pathSearchPageNumber, singleDocumentPath1, singleDocumentPath2 },//tiene conto della pagina risultati della ricerca                 ,
                    PostValues = new List<KeyValuePair<string, string>>() 
                };

                String htmlFromPagePath = GetContent(singleDocumentPath2, cookies);

                string articoliSelectedPattern = string.Concat(@"articoliSelected""", @" type=""checkbox""", @" value=""(.*?)""");

                articoliSelectedList = CreatesListForArticoliSelected(articoliSelectedPattern, htmlFromPagePath); 

                string _articoliSelectedPattern = string.Concat(@"_articoliSelected""", @" type=""hidden""", @" value=""(.*?)"""); 

                List<string> _articoliSelectedList = CreatesListFor_ArticoliSelected(_articoliSelectedPattern, htmlFromPagePath); 

                KeyValuePair<String, String> mySinglePost;

                for (int i = 0; i < articoliSelectedList.Count; i++) //CREO LA PRIMA PARTE DEI DATI DA PASSARE IN POST (VARIE COPPIE articoliSelected, _articoliSelected):
                {
                    string articoliSelected = string.Format("articoliSelected[{0}]", i);

                    string _articoliSelected = string.Format("_articoliSelected[{0}]", i);

                    mySinglePost = new KeyValuePair<string, string>(articoliSelected, articoliSelectedList[i].ToString());

                    item.PostValues.Add(mySinglePost);

                    mySinglePost = new KeyValuePair<string, string>(_articoliSelected, _articoliSelectedList[i].ToString());

                    item.PostValues.Add(mySinglePost);
                }

                var postDataListPattern = new List<string>();

                PopulatesPostParameterList(postDataListPattern); //ORA CONTINUO A POPOLARE LA LISTA DEI DATI DA PASSARE IN POST

                CreatesFullPostData(item, htmlFromPagePath, postDataListPattern, giornoVigenza, meseVigenza, annoVigenza, creaAnteprimaStampa, dataPubblicazioneGazzetta, codiceRedazionale); 

                item.URL = "http://www.normattiva.it/do/atto/export";

               

                //foreach (var matchT in Regex
                //.Match(htmlItem, nameLawPatternRegex,  RegexOptions.IgnoreCase | RegexOptions.Compiled).Cast<Match>())
                //{
                //    item.Id = string.Concat(matchT.Groups[1].Value, matchT.Groups[2].Value, matchT.Groups[3].Value, matchT.Groups[4].Value);
                //}        

                // Instantiate the regular expression object.
               

                countId++;

                result.Add(item);
            }

            return hrefPattern;
        }

        private List<string> GetHtmlList(string pHtmlCode, string pHrefPattern, string[] pCookies)
        {
            using (var client = new WebClient())
            {
                foreach (var match in Regex
                  .Matches(pHtmlCode, pHrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                  .Cast<Match>())
                {
                    count++;
                }

                count = (count / 2) + 1;

                while (count > 0)
                {
                    pHrefPattern = pHrefPattern.Replace(@"<a href=""", string.Empty);

                    string requestCookie = string.Concat(pCookies[1].ToString(), "; ");//inverto l'ordine dei parametri cookie per adeguarli all'ordine con cui vengono inviati nelle request successive alla prima

                    requestCookie = string.Concat(requestCookie, pCookies[0].ToString());

                    string pathN = string.Concat("http://www.normattiva.it", pHrefPattern);

                    pathN = string.Concat(pathN, count.ToString());

                    client.Credentials = CredentialCache.DefaultCredentials;

                    client.Headers["Cookie"] = requestCookie; //valorizzo i cookie

                    String htmlFromPagePath = client.DownloadString(pathN);

                    htmlFromPagePaths.Add(htmlFromPagePath);

                    count--;
                }
            }

            return htmlFromPagePaths;
        }

        private string[] GetCookies(string pUrl)
        {
            using (var client = new WebClient())
            {
                client.DownloadString(pUrl);

                string[] cookies = client.ResponseHeaders.GetValues("Set-Cookie");

                client.CancelAsync();

                return cookies;
            }
        }

        private string GetContent(string pUrl)
        {
            using (var client = new WebClient())
            {
                String htmlCode = client.DownloadString(pUrl);

                client.CancelAsync();

                return htmlCode;
            }
        }

        private string GetContent(IDownloadItem pItem)
        {
            using (var client = new WebClient())
            {
                string requestCookie;

                //string[] cookies = GetCookies(startPath);

                //cookies[0] = cookies[0].Replace("; Path=/", string.Empty); 21 mag 2014

                //cookies[1] = cookies[1].Replace("; Path=/", string.Empty); 21 mag 2014

                requestCookie = string.Concat(cookies[1].ToString(), "; ");//inverto l'ordine dei parametri cookie per adeguarli all'ordine con cui vengono inviati nelle request successive alla prima

                requestCookie = string.Concat(requestCookie, cookies[0].ToString());

                var formData = new NameValueCollection();

                for (int i = 0; i < pItem.PostValues.Count; i++)
                {
                    if (pItem.PostValues[i].Key.Contains("articoli"))
                    {
                        formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);
                    }
                    else
                    {
                        for (int k = 0; k < 6; k++)
                        {
                            switch (k)
                            {

                                case 0:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    break;

                                case 1:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    break;

                                case 2:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    break;

                                case 3:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    break;

                                case 4:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    break;

                                case 5:

                                    formData.Add(pItem.PostValues[i].Key, pItem.PostValues[i].Value);

                                    i = pItem.PostValues.Count;

                                    break;
                            }

                            i++;
                        }
                    }


                }

                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                client.Headers["Cookie"] = requestCookie;

                byte[] responseBytes = null;

                string document = string.Empty; 

                try
                {
                    responseBytes = client.UploadValues(pItem.URL, "POST", formData);

                    document = Encoding.ASCII.GetString(responseBytes);

                    //pPostDataListPattern.Add(string.Concat(@"giornoVigenza""", @" class=""xsmall""", @" type=""text""", @" value=""(.*?)"""));

                    string nameLawPatternRegex = string.Concat(@"<div id=""testa_atto_preview"">\s*<p class=""grassetto""", @" style=""padding:2em", @" 0;"">\s*(LEGGE|DECRETO|DECRETO[ ].*|DECRETO-LEGGE|DECRETO-LEGISLATVO)\s*([0-9][0-9]|[0-9])\s*(.*)\s*([0-9][0-9][0-9][0-9],[ ]n.[ ].*)");

                    Regex r = new Regex(nameLawPatternRegex, RegexOptions.IgnoreCase);

                    Match m = r.Match(document);
                    //item.Id = string.Concat("Legge_", countId.ToString()); //REGEX DANIELE

                    pItem.Id = string.Concat(m.Groups[1].Value + "_", m.Groups[2].Value + "_", m.Groups[3].Value, m.Groups[4].Value);

                    pItem.Id = pItem.Id.Replace(".", string.Empty);

                    pItem.Id = pItem.Id.Replace(",", string.Empty);

                    pItem.Id = pItem.Id.Replace("\r", string.Empty);

                    //pItem.Id.Trim();


                    if (document.Contains("Attenzione: la sessione di ricerca &eacute; scaduta e per questo motivo sei stato riportato alla pagina iniziale"))
                    {
                        _log.Error(string.Concat("IMPOSSIBILE SCARICARE IL DOCUMENTO A CAUSA DI PROBLEMATICHE LEGATE ALLA SESSIONE"));

                        document = null;
                    }
                }
                catch (Exception ex)
                {
                    _log.Info(string.Concat("IMPOSSIBILE SCARICARE IL DOCUMENTO A CAUSA DELL'ERRORE: ", ex.Message.ToUpper()));

                    document = null;
                }
                finally
                {
                    client.CancelAsync();
                }

                return document;
            }
        }

        private string GetContent(string pUrl, string[] pCookies) 
        {
            using (var client = new WebClient())
            {
                string requestCookie = string.Concat(pCookies[1].ToString(), "; ");//inverto l'ordine dei parametri cookie per adeguarli all'ordine con cui vengono inviati nelle request successive alla prima

                requestCookie = string.Concat(requestCookie, pCookies[0].ToString()); 

                client.Credentials = CredentialCache.DefaultCredentials;

                client.Headers["Cookie"] = requestCookie; //valorizzo i cookie

                String htmlCode = client.DownloadString(pUrl);

                client.CancelAsync();

                return htmlCode;
            }
        }

        private List<string> CreatesListForArticoliSelected(string pArticoliSelected, string pHtmlFromPagePath)
        {
            List<string> pArticoliSelectedList = new List<string>();

            foreach (var match in Regex
           .Matches(pHtmlFromPagePath, pArticoliSelected, RegexOptions.IgnoreCase | RegexOptions.Compiled)
           .Cast<Match>())
            {
                pArticoliSelectedList.Add(match.Groups[1].Value);
            }

            return pArticoliSelectedList;
        }

        private List<string> CreatesListFor_ArticoliSelected(string p_ArticoliSelected, string pHtmlFromPagePath)
        {
            List<string> p_ArticoliSelectedSelectedList = new List<string>();

            foreach (var match in Regex
           .Matches(pHtmlFromPagePath, p_ArticoliSelected, RegexOptions.IgnoreCase | RegexOptions.Compiled)
           .Cast<Match>())
            {
                p_ArticoliSelectedSelectedList.Add(match.Groups[1].Value);
            }

            return p_ArticoliSelectedSelectedList;
        }

        private void PopulatesPostParameterList(List<string> pPostDataListPattern)
        {
            pPostDataListPattern.Add(string.Concat(@"giornoVigenza""", @" class=""xsmall""", @" type=""text""", @" value=""(.*?)"""));

            pPostDataListPattern.Add(string.Concat(@"meseVigenza""", @" class=""xsmall""", @" type=""text""", @" value=""(.*?)"""));

            pPostDataListPattern.Add(string.Concat(@"annoVigenza""", @" class=""xsmall""", @" type=""text""", @" value=""(.*?)"""));

            pPostDataListPattern.Add(string.Concat(@"creaAnteprimaStampa""", @"  value=""(.*)"""));

            pPostDataListPattern.Add(string.Concat(@"dataPubblicazioneGazzetta", @"""\s*value=""(.*)"""));

            pPostDataListPattern.Add(string.Concat(@"codiceRedazionale", @"""\s*value=""(.*)"""));
        }

        private void CreatesFullPostData(DownloadItem pItem, String pHtmlFromPagePath, List<string> pPostDataListPattern, string pGiornoVigenza, string pMeseVigenza, string pAnnoVigenza, string pCreaAnteprimaStampa, string pDataPubblicazioneGazzetta, string pCodiceRedazionale)
        {
            KeyValuePair<String, String> mySinglePost;

            for (int i = 0; i < 6; i++)
            {
                Match matchX;

                matchX = Regex.Match(pHtmlFromPagePath, pPostDataListPattern[i].ToString(),

                RegexOptions.IgnoreCase | RegexOptions.Compiled);

                switch (i)
                {

                    case 0:

                        pGiornoVigenza = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("giornoVigenza", pGiornoVigenza);

                        pItem.PostValues.Add(mySinglePost);

                        break;

                    case 1:

                        pMeseVigenza = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("meseVigenza", pMeseVigenza);

                        pItem.PostValues.Add(mySinglePost);

                        break;

                    case 2:
                        pAnnoVigenza = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("annoVigenza", pAnnoVigenza);

                        pItem.PostValues.Add(mySinglePost);

                        break;

                    case 3:
                        pCreaAnteprimaStampa = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("creaAnteprimaStampa", pCreaAnteprimaStampa);

                        pItem.PostValues.Add(mySinglePost);

                        break;

                    case 4:
                        pDataPubblicazioneGazzetta = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("dataPubblicazioneGazzetta", pDataPubblicazioneGazzetta);

                        pItem.PostValues.Add(mySinglePost);

                        break;

                    case 5:
                        pCodiceRedazionale = matchX.Groups[1].Value;

                        mySinglePost = new KeyValuePair<string, string>("codiceRedazionale", pCodiceRedazionale);

                        pItem.PostValues.Add(mySinglePost);

                        break;
                }

            }

        }

        private static NameValueCollection CreatesFullParameterizedUrl(string pMesePubblicazioneDa, string pAnnoPubblicazioneDa, string pMesePubblicazioneA, string pAnnoPubblicazioneA)
        {
            var formData = new NameValueCollection();

            formData.Add("mesePubblicazioneDa", pMesePubblicazioneDa);

            formData.Add("annoPubblicazioneDa", pAnnoPubblicazioneDa);

            formData.Add("mesePubblicazioneA", pMesePubblicazioneA);

            formData.Add("annoPubblicazioneA", pAnnoPubblicazioneA);

            return formData;
        }
        
        #endregion
    }
}
