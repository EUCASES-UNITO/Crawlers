using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unito.Eucases.Crawlers;

namespace Unito.EUCases.Crawlers.EurLex
{
    public class DocumentStore
    {
        public DocumentStore()
        {
            Tags = new List<string>();
        }
        public string Url { get; set; }
        public List<string> Tags { get; set; }
        public int MaxTags
        {
            get
            {
                return Tags.Count;
            }
        }
        public string HTMLcontent { get; set; }
        public byte[] ByteDocument { get; set; }
        public bool IsPdf { get; set; }
        public bool ShouldBeIgnoredWhileIsEmpty { get; set; }

        public string PhisicalName { get; set; }
    }

    public class CrawlerImpl : ICrawler<ParametersEurLex, string>
    {
        private const int MaxItemsToSearch = 20;

        public List<DocumentStore> ListDocumentStore = new List<DocumentStore>();

        public CrawlerImpl(ParametersEurLex parameters)
        {
            Parameters = parameters;
            if (!Directory.Exists(Parameters.WorkFolder))
            {
                Directory.CreateDirectory(Parameters.WorkFolder);
            }
        }
        
        public ParametersEurLex Parameters
        { get; set; }

        public IEnumerable<IDownloadItem> GetDownloadList()
        {
            return OptimizeList(ExecuteSearch(Parameters.Url, Parameters.TagName));
        }
        
        private List<IDownloadItem> ExecuteSearch(string url, string tagName)
        {
            List<IDownloadItem> myList = new List<IDownloadItem>();
            string HtmlResult = string.Empty;
            string[] myCookies;
            string parameterGetListCurrent = string.Empty;
            string localUrl;
            localUrl = url;

            // devo preparare la sessione altrimenti non posso iniziare a cercare

                                
            var localUrl2 = @"http://eur-lex.europa.eu/search.html?instInvStatus=ALL&qid=1401782258387&DTS_DOM=PUBLISHED_IN_OJ&orEUROVOC=DC_DECODED%3D%22{0}%22,DC_ALTLABEL%3D%22{0}%22&type=advanced&lang=it&SUBDOM_INIT=PUBLISHED_IN_OJ&DTS_SUBDOM=PUBLISHED_IN_OJ";
            localUrl = localUrl2;

            localUrl = string.Format(localUrl, tagName);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                HtmlResult = wc.DownloadString(localUrl);
                myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
            }

           

            string regExfPatFindDocumentNumber = @"<span>of<\/span>\&nbsp;(.*)<\/p>";

            var m = Regex.Match(HtmlResult, regExfPatFindDocumentNumber,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
            int paginaMax = 0;

            if (m.Success)
            {
                int numMax = Convert.ToInt32(m.Groups[1].Value);
                paginaMax = numMax / 10;
                int num = 0;
                Math.DivRem(numMax, 10, out num);
                if (num > 0)
                {
                    paginaMax++;
                }
            }

            // Si prende la prima decina di pagine
            string regExfPatFindLink = @"<td colspan=""2"" class=""publicationTitle"" style=""""><a href=""(.*?)""";

            foreach (var match in Regex
                            .Matches(HtmlResult, regExfPatFindLink, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                            .Cast<Match>())
            {
                IDownloadItem myDownload = new DownloadItem();
                myDownload.URL = match.Groups[1].Value;
                myDownload.URL = myDownload.URL.Replace("&amp;", "&");
                myList.Add(myDownload);
            }

            string regExPathFindPageLink = @"<a href=""(.*?)>2<";

            for (int i = 2; i < paginaMax && i < MaxItemsToSearch; i++)
            {
                var matchRegexPage = Regex.Match(HtmlResult, regExPathFindPageLink,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));

                if (matchRegexPage.Success)
                {
                    string urlPage=  string.Empty;
                    if(i==2)
                        urlPage = matchRegexPage.Groups[1].Value;
                    if (i > 2)
                        urlPage = matchRegexPage.Groups[3].Value;
                     
                    string tagUrl = @"http://eur-lex.europa.eu/";
                    urlPage = urlPage.Replace(@"./", tagUrl);
                    urlPage = urlPage.Replace("&amp;", "&");
                    urlPage = urlPage.Replace(@"""", string.Empty );

                    regExPathFindPageLink = @"<a href=""(.*?)>&nbsp;" + i + @"<(.*?)<a href=""(.*?)>" + (i+1).ToString() + "<";                   

                    using (WebClient wc = new WebClient())
                    {

                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        HtmlResult = wc.DownloadString(urlPage);
                        myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
                        // si prende la decina di link della pagina appena caricata 
                        if (!Directory.Exists(Path.Combine(Parameters.WorkFolder, tagName)))
                            Directory.CreateDirectory(Path.Combine(Parameters.WorkFolder, tagName));

                        File.WriteAllText(Path.Combine(Parameters.WorkFolder, tagName,"SearchPage" + tagName + i + ".html") , HtmlResult); 

                        foreach (var matchLink in Regex
                                        .Matches(HtmlResult, regExfPatFindLink, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                                        .Cast<Match>())
                        {
                            IDownloadItem myDownload = new DownloadItem();                            
                            myDownload.URL = matchLink.Groups[1].Value;                            
                            myDownload.URL = myDownload.URL.Replace("&amp;", "&");
                            myList.Add(myDownload);
                        }
                    }
                }
            }
            return myList;
        }

        public IDownloadResult<string> Download(IDownloadItem item)
        {
            throw new NotImplementedException();
        }

        public List<IDownloadItem> OptimizeList(List<IDownloadItem> toOptimize)
        {
            string HtmlResultDocument = string.Empty;
            string HtmlResultTag = string.Empty;
            string[] myCookies;
            int counterProcessedDocument = 0;

            List<IDownloadItem> returnList = new List<IDownloadItem>();

            // per ogni documento mi devo caricare i tag
            
            foreach(var item in toOptimize)
            {
                counterProcessedDocument++;
                int indexOfUri = item.URL.IndexOf(@"?uri=") +5;                
                int indexOfqid = item.URL.IndexOf(@"&qid=") + 5 ;
                int indexOfRid = item.URL.IndexOf(@"&rid=");
                int lenghtUri = indexOfqid - indexOfUri-5;
                int lenghtQid = indexOfRid - indexOfqid;

                string id1 = item.URL.Substring(indexOfUri, lenghtUri).Replace(":","_");
                string id2 = item.URL.Substring(indexOfqid, lenghtQid);

                string tagUrl = @"http://eur-lex.europa.eu/";
                item.URL = item.URL.Replace(@"./", tagUrl);

                using (WebClient wc = new WebClient())
                {
                    
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    HtmlResultDocument = wc.DownloadString(item.URL);
                    myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
                }
                // Questo Html e' buono. Contiene il testo del documento.
                // Lo dovrei salvare e mettermelo da parte// in realta' no, perche' ancora non so se fa parte dei top 10 
                var nomefile = string.Concat("URI-", id1, "-Quid-", id2, ".html").Replace(@"/", "-").Replace(@"\", "-");

                // intanto creo il documento che metto dentro l'array e comincio a segnarmi il content. i nomi glieli do dopo
                DocumentStore doc = new DocumentStore();
                doc.HTMLcontent = HtmlResultDocument;

                // devo verificare che non sia testo vuoto, se e' vuoto allora cerco il pdf in inglese
                string regexFindIfIsEmpty = @"The text is not displayed because the HTML version of the document has not been published yet or does not exist.";
                var filePath = Path.Combine(Parameters.WorkFolder, Parameters.TagName, nomefile);

                var z = Regex.Match(HtmlResultDocument, regexFindIfIsEmpty,
                               RegexOptions.IgnoreCase | RegexOptions.Compiled,
                               TimeSpan.FromSeconds(1));
                if (z.Success)
                {
                    string linkUrl = item.URL.Replace(@"/AUTO/", "/en/TXT/PDF/");
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/pdf";                        
                        try
                        {
                            doc.IsPdf = true;
                            wc.DownloadFile(linkUrl, filePath.Replace(".html", ".pdf"));
                            doc.ByteDocument = wc.DownloadData(linkUrl);                            
                            myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
                        }
                        catch(Exception ex){
                            doc.ShouldBeIgnoredWhileIsEmpty = true;
                        }                       
                    }
                }
                
                try
                {
                    if(!doc.IsPdf)
                    File.WriteAllText(filePath, HtmlResultDocument);
                }
                catch (Exception ex)
                {
                    
                    var guid = Guid.NewGuid();
                    string fileFallito = guid.ToString() + "FileFallito.txt";
                    File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, "contenuto" + nomefile), HtmlResultDocument);
                    File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, "nome" + nomefile), nomefile);
                    File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, "errore" + nomefile), ex.Message);                                        
                }
                
                

                // adesso mi devo spostare lateralmente di tab per andare a prendere i tag
                string regExfPatFindLinkTab = @"<li class=""doc-tab-biblio""><a href="".\/..\/..\/..(.*?)""";

                var m = Regex.Match(HtmlResultDocument, regExfPatFindLinkTab,
                                RegexOptions.IgnoreCase | RegexOptions.Compiled,
                                TimeSpan.FromSeconds(1));
                if(m.Success)
                {
                    string linkUrl = string.Concat( @"http://eur-lex.europa.eu",m.Groups[1].Value);
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        HtmlResultTag = wc.DownloadString(linkUrl);
                        myCookies = wc.ResponseHeaders.GetValues("Set-cookie");
                    }
                }

                string regExpTag = @"<a href="".\/..\/..\/..\/search.html\?type=advanced(.*?)>(.*?)<";

                
                doc.Url = item.URL;
                foreach (var match in Regex
                                .Matches(HtmlResultTag, regExpTag, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                                .Cast<Match>())
                {

                    string tag = match.Groups[2].Value;
                    doc.Tags.Add(tag);
                }
                if (doc.Tags.Count == 0)
                {
                    string stop = string.Empty;
                }
                doc.PhisicalName = string.Concat("URI-", id1, "-Quid-", id2, ".html").Replace(@"/", "-").Replace(@"\", "-"); 
                ListDocumentStore.Add(doc);
              
                // mi devo segnare in un array il nome del documento le parole associate e il numero di parole chiave
                // cosi lentamente popolo l'array con le informazioni necessarie.
                // poi scelgo il report.
                
            }
            // devo selezionare i top 10 documenti che abbiano minori tag possibili
            // devo fare una query Linq

            var listSelected = ListDocumentStore.Where(x=>!x.ShouldBeIgnoredWhileIsEmpty).OrderBy(x => x.MaxTags).Take(30);
            // adesso devo creare una directory con i dati dei file

            foreach(var itemDocStore in listSelected)
            {
                foreach (var itemTag in itemDocStore.Tags)
	            {
                    if( !Directory.Exists(Path.Combine( Parameters.WorkFolder,Parameters.TagName, itemTag)))
                    {
                        Directory.CreateDirectory(Path.Combine( Parameters.WorkFolder,Parameters.TagName, itemTag));                        
                    }
                    var filePathTagged = Path.Combine(Parameters.WorkFolder, Parameters.TagName, itemTag, itemDocStore.PhisicalName);
                    try
                    {
                        if(!itemDocStore.IsPdf && !itemDocStore.ShouldBeIgnoredWhileIsEmpty)
                        { 
                            File.WriteAllText(filePathTagged, itemDocStore.HTMLcontent);
                        }else
                        {
                            if (!itemDocStore.ShouldBeIgnoredWhileIsEmpty)
                            { 
                                System.IO.FileStream _FileStream =
                                    new System.IO.FileStream(filePathTagged.Replace(".html",".pdf"), System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write);
                                // Writes a block of bytes to this stream using data from
                                // a byte array.
                                _FileStream.Write(itemDocStore.ByteDocument, 0, itemDocStore.ByteDocument.Length);
                                // close file stream
                                _FileStream.Close();
                            }

                        }
                    }
                    catch(Exception ex)
                    {
                        var guid = Guid.NewGuid();
                        string fileFallito = guid.ToString() + "FileFallito.txt";
                        File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, itemTag, "contenuto" + fileFallito), HtmlResultDocument);
                        File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, itemTag, "nome" + fileFallito), filePathTagged);
                        File.WriteAllText(Path.Combine(Parameters.WorkFolder, Parameters.TagName, itemTag, "errore" + fileFallito), ex.Message);
                    }
	            }
            }
            // li devo mettere tante volte per quante sono le categorie che trovo dentro
            return returnList;
        }
    }
}
