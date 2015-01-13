using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unito.Eucases.Crawlers;
using Unito.EUCases.CrawlersUploader;
using Unito.EUCases.CrawlersUploader.DAL;

namespace Unito.EUCases.Crawlers.GiustiziaAmministrativaWorker
{
    public partial class GiustiziaAmministrativaForms : Form
    {
        private string _serviceUrl;

        public string ServiceUrl
        {
            get { return _serviceUrl; }
            set { _serviceUrl = value; }
        }

        private string _workingFolder;

        private bool _CheckIfTheFileHasToBeRenewed;

        public bool CheckIfTheFileHasToBeRenewed
        {
            get { return _CheckIfTheFileHasToBeRenewed; }
            set { _CheckIfTheFileHasToBeRenewed = value; }
        }

        public string WorkingFolder
        {
            get { return _workingFolder; }
            set { _workingFolder = value; }
        }

        string _urlForm;

        public string UrlForm
        {
            get { return _urlForm; }
            set { _urlForm = value; }
        }
        int _startYear;

        public int StartYear
        {
            get { return _startYear; }
            set { _startYear = value; }
        }
        int _endYear;

        public int EndYear
        {
            get { return _endYear; }
            set { _endYear = value; }
        }

        private int _maxDoc;

        public int MaxDoc
        {
            get { return _maxDoc; }
            set { _maxDoc = value; }
        }

        private int _MaxRandomWait;

        public int MaxRandomWait
        {
            get { return _MaxRandomWait; }
            set { _MaxRandomWait = value; }
        }
        
        int _currentYear;
        int _numeroDocumento = 1;
        bool _isInWhiteLoop = false;
        int _whiteLoopCounter = 1;
        List<IDownloadItem> _myDownloadItemList ;
        IDownloadItem _CurrentDownloadItem;        
        int _CurrentIndexDownloadItem;
        ManageUpload CrawlerUploader;

        public GiustiziaAmministrativaForms()
        {
            InitializeComponent();
            webBrowser2.DocumentCompleted += webBrowser2_DocumentCompleted;
            webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
        }

        void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _CurrentIndexDownloadItem++;
            string html = webBrowser2.Document.Body.OuterHtml;
            
            var result = new DownloadResultBase<string>
            {
                Request = _CurrentDownloadItem
            };
            result.Content = html;

            var filePath = Path.Combine(_workingFolder, string.Concat(result.Request.Id, ".html"));
            File.WriteAllText(filePath, result.Content);
            var filePathTxt = Path.Combine(_workingFolder, string.Concat(result.Request.Id, ".txt"));
            File.WriteAllText(filePathTxt, result.Request.URL);

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
            CrawlerUploader.UrlService = _serviceUrl;
            CrawlerUploader.UploadAllFilesToService();

            if (_CurrentIndexDownloadItem < _myDownloadItemList.Count)
            { 
                _CurrentDownloadItem = _myDownloadItemList[_CurrentIndexDownloadItem];
                webBrowser2.Navigate(_CurrentDownloadItem.URL.Replace("&amp;", "&"));
            }

        }

        private void GetDownloadList()
        {
            ExecuteSearch(_urlForm, _startYear, _endYear);           
        }

        private void ExecuteSearch(string url, int fromYear, int toYear)
        {            
            var returnList = new List<IDownloadItem>();
            
            _myDownloadItemList = new List<IDownloadItem>();
                         
                string HtmlResult = string.Empty;
                string myParameters = @"tipoRicerca=Provvedimenti&FullText=&FullTextA=&FullTextAdvanced=&advInNotParole=&advInFrase=&ResultCount=60&ordinaPer=xNumeroDocumento&xTipoDocumento=PROVVEDIMENTI&xTipoSubProvvedimento=&xTipoProvvedimento=&xSede=&xTipoProvvedimentoDecisione=XXX"
                                        + @"&xNumeroDocumento="
                                        + _currentYear.ToString() + _numeroDocumento.ToString("D5")
                                        + @"&xAnno="
                                        + _currentYear.ToString() +
                                        "&xNProvv5=&PageNumber=&StartRow=&EndRow=&advanced=false";
                url += myParameters;
                
                webBrowser1.Navigate(url);                                
        }

        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!_isInWhiteLoop)
            {
                WebBrowser myWebBrowser = (WebBrowser)sender;
                string HtmlResult = myWebBrowser.Document.Body.OuterHtml;

                // devo prendermi il numero totale di documenti per anno
                string HRefPattern = @"<a href=""javascript:addAnno[^>]*>(.*)a>";

                Match m;
                m = Regex.Match(HtmlResult, HRefPattern,
                                RegexOptions.IgnoreCase | RegexOptions.Singleline,
                                TimeSpan.FromSeconds(1));

                int totalNumberSearch = 0;
                if (m.Success) // ho trovato qualcosa
                {
                    string checkString = m.Groups[1].Value;
                    int startFrom = checkString.IndexOf(_currentYear.ToString()) + _currentYear.ToString().Length + 2;
                    int stopTo = checkString.IndexOf(")");
                    int lenghtTotal = stopTo - startFrom;
                    string subCheckString = checkString.Substring(startFrom, lenghtTotal);
                    try
                    {
                        totalNumberSearch = Convert.ToInt16(subCheckString);
                    }
                    catch
                    {
                        _whiteLoopCounter++;
                    }
                }

                if (_whiteLoopCounter > 20) { _isInWhiteLoop = true; return; }

                // e mi sono agganciato il numero di documenti totali.
                if (totalNumberSearch <= 60)
                {
                    string myRefPattern = @"<a href=""\/cdsintra\/cdsintra\/AmministrazionePortale[^>]*>(.*)>";


                    foreach (var match in Regex
                                        .Matches(HtmlResult, myRefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
                                        .Cast<Match>())
                    {
                        string myValue = match.Groups[0].Value;                        
                        int startPath = myValue.IndexOf("<A href=") + 9;
                        int lenghtPath = myValue.IndexOf(">") - startPath - 1;
                        string relativePath = myValue.Substring(startPath, lenghtPath);
                        string myFinalAddress = string.Concat(@"https://94.86.40.196/", relativePath.Replace(@"&amp;","&"));

                        

                        DownloadItem myItem = new DownloadItem();

                        int startTipoDocument = myValue.IndexOf("<BR>Tipo documento:") + "<BR>Tipo documento:".Length;
                        int lenthTipoDocumento = myValue.IndexOf(",<B>") - startTipoDocument;
                        string tipoDocumento;
                        tipoDocumento = myValue.Substring(startTipoDocument, lenthTipoDocumento);

                        int startVariab = myValue.IndexOf(tipoDocumento + ",<B>") + tipoDocumento.Length + ",<B>".Length;
                        int lenghtVariab = myValue.IndexOf("</B> ,sede di <B>") - startVariab;
                        string variab;
                        variab = myValue.Substring(startVariab, lenghtVariab);

                        int startSede = myValue.IndexOf("</B> ,sede di <B>") + @"</B> ,sede di <B>".Length;
                        int lenghtSede = myValue.IndexOf("</B> ,sezione <B>") - startSede;
                        string sede = myValue.Substring(startSede, lenghtSede);

                        int startSezione = myValue.IndexOf("</B> ,sezione <B>") + "</B> ,sezione <B>".Length;
                        int lenghtSezione = myValue.IndexOf("</B> ,numero provv.: <B>") - startSezione;
                        string sezione = myValue.Substring(startSezione, lenghtSezione);

                        int startNumeroProvv = myValue.IndexOf("</B> ,numero provv.: <B>") + "</B> ,numero provv.: <B>".Length;
                        int lenghtNumeroProvv = myValue.IndexOf("</B>,") - startNumeroProvv;
                        if (lenghtNumeroProvv < 0) lenghtNumeroProvv = myValue.IndexOf("</B><BR>") - startNumeroProvv;
                        string numeroProvv = myValue.Substring(startNumeroProvv, lenghtNumeroProvv);

                        int startNumeroRicorso = myValue.IndexOf("<BR>Numero ricorso:<B>") + "<BR>Numero ricorso:<B>".Length;
                        int lenghtNumeroRicorso = myValue.Length - 4 - startNumeroRicorso;
                        string NumeroRicorso = myValue.Substring(startNumeroRicorso, lenghtNumeroRicorso);

                        myItem.Id = string.Concat(tipoDocumento, "-", variab, "-", sede, "-", sezione, "-", numeroProvv, "-", NumeroRicorso).Replace(" ", string.Empty);
                        myItem.URL = myFinalAddress;
                        _myDownloadItemList.Add(myItem);

                    }
                }
                else// se ce ne sono piu\ di 60 dovro' paginare            
                {

                }
            }// chiusura del whiteloop
            else // Sono in white loop, devo incrementare il current year
            {
                if (_currentYear < _startYear) _currentYear++;
            }

            _numeroDocumento++;
            

            if (_myDownloadItemList.Count > _maxDoc && _maxDoc != 0) 
            {
                _isInWhiteLoop = true;
                GetItems();
                return;
            }

            if (!_isInWhiteLoop)
            {
                string url = _urlForm;
                string HtmlResult = string.Empty;
                string myParameters = @"tipoRicerca=Provvedimenti&FullText=&FullTextA=&FullTextAdvanced=&advInNotParole=&advInFrase=&ResultCount=60&ordinaPer=xNumeroDocumento&xTipoDocumento=PROVVEDIMENTI&xTipoSubProvvedimento=&xTipoProvvedimento=&xSede=&xTipoProvvedimentoDecisione=XXX"
                                        + @"&xNumeroDocumento="
                                        + _currentYear.ToString() + _numeroDocumento.ToString("D5")
                                       + @"&xAnno="
                                      + _currentYear.ToString() +
                                        "&xNProvv5=&PageNumber=&StartRow=&EndRow=&advanced=false";
                url += myParameters;
                webBrowser1.Navigate(url.Replace("&amp;", "&"));
            }
        }// chiudura della funzione di Caricamento dell'html completato
        

        private void GetItems()
        {
            IStoreMetadata _storeMetadata = new StoreMetadaOnFile(_workingFolder + "\\database");

            bool HaveToBeDownloaded = _CheckIfTheFileHasToBeRenewed;

             if (!_storeMetadata.ExistsFile(_CurrentDownloadItem.Id) || HaveToBeDownloaded)
             {
                 if (_MaxRandomWait != 0)
                 {
                     Random rnd = new Random();
                     int seconds = rnd.Next(1, _MaxRandomWait) * 1000;
                     System.Threading.Thread.Sleep(seconds);
                 }

                 _CurrentDownloadItem = (IDownloadItem)_myDownloadItemList[_CurrentIndexDownloadItem];
                 webBrowser2.Navigate(_CurrentDownloadItem.URL.Replace("&amp;", "&"));
             }
        }

        private void btnDownload_Click_1(object sender, EventArgs e)
        {
            CrawlerUploader = new ManageUpload("GiustiziaAmministativa", "Italian", _workingFolder);

            _currentYear = _startYear;
            ExecuteSearch(_urlForm, _currentYear, _currentYear);   
        }

        
    }
}
