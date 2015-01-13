using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.CrawlersUploader.DAL;
using System.IO.Compression;
using System.IO;
using Ionic.Zip;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace Unito.EUCases.CrawlersUploader
{
    public class ManageUpload:IManageUploading
    {
        List<documentgroup> _documentgroups;        
        List<documentgroupDocument> _documentsAdd;
        List<documentgroupDocument> _documentsUpd;
        String _pathAddFiles;
        String _pathDelFiles;
        String _pathNoneFiles;
        String _pathUpdateFiles;
        String _outputFolderAdd;
        String _outputFolderUpd;
        IStoreMetadata _storeMetadata;
        String _FileNameZipAdd;
        String _FileNameZipUpd;
        string _SentDataFolderAdd;
        string _SentDataFolderUpd;
        string _crawler;
        string _language;
        string _urlService;
        string _pathError;

        public string UrlService
        {
            get { return _urlService; }
            set { _urlService = value; }
        }


        public ManageUpload(string crawler, string language, string workingFolder)
        {
            _documentgroups = new List<documentgroup>(); 
            
            _documentsAdd = new List<documentgroupDocument>();
            _documentsUpd = new List<documentgroupDocument>();

            string temp = workingFolder;                

            _pathAddFiles = string.Concat(temp,@"\","AddServiceEUCases");            
            _pathDelFiles = string.Concat(temp,@"\","DelServiceEUCases");            
            _pathNoneFiles = string.Concat(temp,@"\","NoneServiceEUCases");            
            _pathUpdateFiles = string.Concat(temp,@"\","UpdateServiceEUCases");
            _storeMetadata = new StoreMetadaOnFile(temp + "\\database");
            _outputFolderAdd = string.Concat(temp, @"\", "OutputReadyToSendServiceEUCasesAdd");
            _outputFolderUpd = string.Concat(temp, @"\", "OutputReadyToSendServiceEUCasesUpd");
            _SentDataFolderAdd = string.Concat(temp, @"\", "SentDataToServiceEUCasesAdd");
            _SentDataFolderUpd = string.Concat(temp, @"\", "SentDataToServiceEUCasesUpd");
            _pathError = string.Concat(temp, @"\", "SentDataInError");

            CleanDirectory(_pathAddFiles);
            CleanDirectory(_pathDelFiles);
            CleanDirectory(_pathNoneFiles);
            CleanDirectory(_pathUpdateFiles);
            CleanDirectory(_outputFolderAdd);
            CleanDirectory(_outputFolderUpd);
            
            if (!Directory.Exists(_SentDataFolderAdd)) Directory.CreateDirectory(_SentDataFolderAdd);
            if (!Directory.Exists(_SentDataFolderUpd)) Directory.CreateDirectory(_SentDataFolderUpd);
            if (!Directory.Exists(_pathError)) Directory.CreateDirectory(_pathError);
                       
            _crawler = crawler;
            _language = language;
            _FileNameZipAdd = GetFileName(documentgroupOperation.Add, crawler, _FileNameZipAdd);
            _FileNameZipUpd = GetFileName(documentgroupOperation.Upd, crawler, _FileNameZipUpd);
            
        }

        private string GetFileName(documentgroupOperation docOp, string crawler, string nameFile)
        {
            if (nameFile == null)
            {
                Guid myGuid = Guid.NewGuid();
                return string.Concat(docOp.ToString(), DateTime.Now.Year, "-", DateTime.Now.Month, "-", DateTime.Now.Day, "_", DateTime.Now.Hour, "_", DateTime.Now.Minute, "-"
                    , crawler, myGuid.ToString());
            }
            else
            {
                return nameFile;
            }            
        }

        public void SetZippedFileName(string fileName)
        {
            _FileNameZipAdd = fileName ;
            _FileNameZipUpd = fileName ;
        }
        
        private documentgroup prepareDocumentGroup( documentgroupOperation docGrOp)
        {
            documentgroup dg = new documentgroup();
            dg.date = DateTime.Now;
            dg.crawler = _crawler;
            dg.lang = _language;
            dg.format = documentgroupFormat.applicationzip;
            dg.filename = GetFileName(docGrOp, _crawler, _FileNameZipAdd) + ".zip"; 
            dg.identifier =  Guid.NewGuid().ToString();
            dg.operation = docGrOp;
            return dg;
        }

        private documentgroupDocument prepareDocumentGroupDocument
            (documentgroupDocumentFormat format, string filename, string identifier, documentgroupDocumentOperation docOp, string url, string MD5)
        {
            documentgroupDocument myDoc = new documentgroupDocument();
            myDoc.file = filename;
            myDoc.format = format;
            myDoc.identifier = identifier;
            myDoc.md5 = MD5;
            myDoc.operation = docOp;
            myDoc.url = url;

            return myDoc;
        }

        private byte[] ZippingFolder(string pathfolder, string outputFolder, string fileNameZip)
        {
            string destinationPathFile;
            destinationPathFile = outputFolder + "\\" + fileNameZip + ".zip"; 
            using (ZipFile zip = new ZipFile())
                    {
                        // Add all files in directory
                        foreach (var file in Directory.EnumerateFiles(pathfolder))
                           zip.AddFile(file,"\\");
                        // Save to output filename
                        zip.Save(destinationPathFile);
                    }
            return GetBytesFromFile(destinationPathFile);
        }

        private byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = null;
            try
            {
                fs = File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

        }
        
        private bool SendDataToEU(byte[] zippedFile, documentgroup docGroup, out string error )
        {
            error = string.Empty;
            bool isOk = false;
            ServiceEUCases.ServiceEUCasesClient EUClient = new ServiceEUCases.ServiceEUCasesClient();
            ServiceEUCases.UploadDocumentGroup EUGroupAdd = new ServiceEUCases.UploadDocumentGroup();
            EUGroupAdd.Data = zippedFile;
            EUGroupAdd.MetaInfo = SerializeDocumentGroup(docGroup);
            string returnString =  EUClient.UploadFile(EUGroupAdd);
            if (returnString == "Ok")
            {
                isOk = true;

            }
            else { error = returnString; }
            return isOk;
        }

        private string SerializeDocumentGroup(documentgroup docGroup)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(documentgroup));
            StringWriter swAdd = new StringWriter();
            serializer.Serialize(swAdd, docGroup);
            return swAdd.ToString();
        }

        public void PrepareToSendData()
        {
            _documentgroups.Add(prepareDocumentGroup( documentgroupOperation.Add));
            _documentgroups.Add(prepareDocumentGroup( documentgroupOperation.Upd));
        }

        public List<ReturnBatchExecuted> UploadAllFilesToService()
        {
            
            ServiceEUCases.ServiceEUCasesClient _client = new ServiceEUCases.ServiceEUCasesClient();                       

            List<ReturnBatchExecuted> returnList = new List<ReturnBatchExecuted>();            

            ServiceEUCases.UploadDocumentGroup _group = new ServiceEUCases.UploadDocumentGroup();
            
            // comprimere i dati nelle 4 cartelle
            byte[] zippedFileAdd;
            byte[] zippedFileUpdate;
            zippedFileAdd = ZippingFolder(_pathAddFiles, _outputFolderAdd, _FileNameZipAdd);
            zippedFileUpdate = ZippingFolder(_pathUpdateFiles, _outputFolderUpd, _FileNameZipUpd);
            // preparazione dei metadati accumulati
            _documentgroups[0].document = _documentsAdd.ToArray();
            //_documentgroups[1].document = _documentsUpd.ToArray();

            // spedire i dati
            bool isSentOk = false;
            string error;
            bool isAllreadySent = false;
            
            if (_documentgroups[0].document.Count() == 0)
            {
                isAllreadySent = true;
            }
            
            //SendDataToEU(zippedFileUpdate, _documentgroups[1]);
            if(!isAllreadySent)
            {
                isSentOk = SendDataToEU(zippedFileAdd, _documentgroups[0], out error);

                // restituire al worker il blob di dati perche' sia archiviato
                ReturnBatchExecuted addBatch = new ReturnBatchExecuted();
                ReturnBatchExecuted updateBatch = new ReturnBatchExecuted();
                                                                 
                if (isSentOk)
                {
                    XmlSerializer serializer;
                    if (File.Exists(string.Concat(_outputFolderAdd, "\\", _FileNameZipAdd, ".zip")) && _documentsAdd.Count() > 0)
                    {
                        File.Move(string.Concat(_outputFolderAdd, "\\", _FileNameZipAdd, ".zip"), string.Concat(_SentDataFolderAdd, "\\", _FileNameZipAdd, ".zip"));
                        // persisto il file xml che ho spedito al servizio
                        string sentDataGroupDocumentAdd = string.Concat(_SentDataFolderAdd, "\\", _FileNameZipAdd, ".xml");
                        serializer = new XmlSerializer(typeof(documentgroup));
                        StreamWriter swAdd = new StreamWriter(sentDataGroupDocumentAdd);
                        serializer.Serialize(swAdd, _documentgroups[0]);
                        swAdd.Close();
                    }                    
                    // segnarseli sul database
                    _storeMetadata.SaveData();
                }
                if (!isSentOk)
                {
                    XmlSerializer serializer;
                    if (File.Exists(string.Concat(_outputFolderAdd, "\\", _FileNameZipAdd, ".zip")) && _documentsAdd.Count() > 0)
                    {
                        File.Move(string.Concat(_outputFolderAdd, "\\", _FileNameZipAdd, ".zip"), string.Concat(_pathError, "\\", _FileNameZipAdd, ".zip"));
                        // persisto il file xml che ho spedito al servizio
                        string sentDataGroupDocumentAdd = string.Concat(_pathError, "\\", _FileNameZipAdd, ".xml");
                        serializer = new XmlSerializer(typeof(documentgroup));
                        StreamWriter swAdd = new StreamWriter(sentDataGroupDocumentAdd);
                        serializer.Serialize(swAdd, _documentgroups[0]);
                        swAdd.Close();
                    }                                      
                }
                addBatch.nameZippedFile = _FileNameZipAdd;
                addBatch.XMLDocumentGroup = SerializeDocumentGroup(_documentgroups[0]);
                addBatch.ZippedFile = zippedFileAdd;
                returnList.Add(addBatch);

            }
            // procedure di pulizia della applicazione
            CleanStructures();
            

            return returnList;

            // cancellare i documenti delle directory interne preparati per upload
        }

        private void CleanDirectory( string directoryPath)
        {
            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath,true);
            Directory.CreateDirectory(directoryPath);
        }

        private void CleanStructures()
        {
            
            CleanDirectory(_pathAddFiles);
            CleanDirectory(_pathUpdateFiles);           
            CleanDirectory(_outputFolderAdd);
            CleanDirectory(_outputFolderUpd);
            _documentgroups = new List<documentgroup>();            
            _documentsAdd = new List<documentgroupDocument>();
            _documentsUpd = new List<documentgroupDocument>();
        }

        public bool UploadFile(string content, string IdDocument, string sMD5, string url, documentgroupFormat formatPar)
        {
            bool isInDB = true;

            if (_storeMetadata.ExistsFile(IdDocument)) // il documento esiste, devo verificare se e' cambiato
            {
                if (!_storeMetadata.CheckFileIsEqual(IdDocument, sMD5)) // il documento e' cambiato
                {
                    isInDB = false;
                    #region UpdateData
                    #region  mettilo nel folder degli update
                                        
                    string pathfileName = string.Empty;
                    if (formatPar == documentgroupFormat.texthtml) pathfileName = _pathUpdateFiles + "\\" + IdDocument + ".html";
                    if (formatPar == documentgroupFormat.textxml) pathfileName = _pathUpdateFiles + "\\" + IdDocument + ".xml";

                    File.WriteAllText(pathfileName, content);
                    
                    byte[] fileMd5;
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(pathfileName))
                        {
                            fileMd5 = md5.ComputeHash(stream);
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < fileMd5.Length; i++)
                    {
                        sb.Append(fileMd5[i].ToString("x2"));
                    }

                    #endregion
                    #region Aggiorna lo stack del documentgroup degli update

                    documentgroupDocument doc = new documentgroupDocument
                    {
                        file = pathfileName,
                        format = documentgroupDocumentFormat.texthtml,
                        identifier = new Guid().ToString(),
                        md5 = fileMd5.ToString(),
                        operation = documentgroupDocumentOperation.Upd,
                        url = url
                    };
                    _documentsUpd.Add(doc);// array per EUservice
                    #endregion
                    // Aggiorna il database
                    _storeMetadata.UpdateFile(IdDocument, sMD5);
                    #endregion
                    // aggiorno il

                }// fine attivita' per documento in update
            }
            else // il documento non esiste, quindi e' nuovo
            {
                isInDB = false;
                // mettilo nel folder degli ADD
                #region  mettilo nel folder degli ADD
                string pathfileName = string.Empty;
                if (formatPar == documentgroupFormat.texthtml) pathfileName = _pathAddFiles + "\\" + IdDocument + ".html";
                if (formatPar == documentgroupFormat.textxml) pathfileName = _pathAddFiles + "\\" + IdDocument + ".xml";
                File.WriteAllText(pathfileName, content);

                byte[] fileMd5;
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(pathfileName))
                    {
                        fileMd5 = md5.ComputeHash(stream);
                    }
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < fileMd5.Length; i++)
                {
                    sb.Append(fileMd5[i].ToString("x2"));
                }

                #endregion

                // aggiorna lo stack del documentgroup degli add                
                documentgroupDocument newDoc = new documentgroupDocument();
                if (formatPar == documentgroupFormat.texthtml) newDoc.file = _FileNameZipAdd + ".html";
                if (formatPar == documentgroupFormat.textxml) newDoc.file = _FileNameZipAdd + ".xml";
                newDoc.format = documentgroupDocumentFormat.texthtml;
                Guid myGuid = Guid.NewGuid();
                newDoc.identifier = myGuid.ToString();
                newDoc.md5 = sb.ToString();
                newDoc.operation = documentgroupDocumentOperation.Add;
                newDoc.url = url;

                _documentsAdd.Add(newDoc);

                // Aggiorna il database
                _storeMetadata.AddFile(IdDocument, sMD5);

            }// fine attivita' per documento in ADD

            return isInDB;
        }

        public bool UploadFileBinary(byte[] content, string IdDocument, string sMD5, string url, documentgroupFormat format)
        {
            bool isInDB = true;

            if (_storeMetadata.ExistsFile(IdDocument)) // il documento esiste, devo verificare se e' cambiato
            {
                if (!_storeMetadata.CheckFileIsEqual(IdDocument, sMD5)) // il documento e' cambiato
                {
                    isInDB = false;
                    #region UpdateData
                    #region  mettilo nel folder degli update

                    string pathfileName = string.Empty;
                    pathfileName = _pathUpdateFiles + "\\" + IdDocument + ".pdf";

                    System.IO.FileStream _FileStream =
                                    new System.IO.FileStream(pathfileName, System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write);
                    _FileStream.Write(content, 0, content.Length);
                    _FileStream.Close();

                    

                    byte[] fileMd5;
                    using (var md5 = MD5.Create())
                    {
                        using (var stream = File.OpenRead(pathfileName))
                        {
                            fileMd5 = md5.ComputeHash(stream);
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < fileMd5.Length; i++)
                    {
                        sb.Append(fileMd5[i].ToString("x2"));
                    }

                    #endregion
                    #region Aggiorna lo stack del documentgroup degli update
                    documentgroupDocument doc = new documentgroupDocument
                    {
                        file = pathfileName,
                        format = documentgroupDocumentFormat.applicationpdf,
                        identifier = new Guid().ToString(),
                        md5 = fileMd5.ToString(),
                        operation = documentgroupDocumentOperation.Upd,
                        url = url
                    };
                    _documentsUpd.Add(doc);// array per EUservice
                    #endregion
                    // Aggiorna il database
                    _storeMetadata.UpdateFile(IdDocument, sMD5);
                    #endregion
                    // aggiorno il

                }// fine attivita' per documento in update
            }
            else // il documento non esiste, quindi e' nuovo
            {
                isInDB = false;
                // mettilo nel folder degli ADD
                #region  mettilo nel folder degli ADD
                string pathfileName = string.Empty;
                pathfileName = _pathAddFiles + "\\" + IdDocument + ".pdf";
                
                System.IO.FileStream _FileStream =
                                    new System.IO.FileStream(pathfileName, System.IO.FileMode.Create,
                                    System.IO.FileAccess.Write);
                _FileStream.Write(content, 0, content.Length);
                _FileStream.Close();                

                byte[] fileMd5;
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(pathfileName))
                    {
                        fileMd5 = md5.ComputeHash(stream);
                    }
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < fileMd5.Length; i++)
                {
                    sb.Append(fileMd5[i].ToString("x2"));
                }

                #endregion

                // aggiorna lo stack del documentgroup degli add                
                documentgroupDocument newDoc = new documentgroupDocument();
                 newDoc.file = _FileNameZipAdd + ".pdf";                
                newDoc.format = documentgroupDocumentFormat.applicationpdf;
                Guid myGuid = Guid.NewGuid();
                newDoc.identifier = myGuid.ToString();
                newDoc.md5 = sb.ToString();
                newDoc.operation = documentgroupDocumentOperation.Add;
                newDoc.url = url;

                _documentsAdd.Add(newDoc);

                // Aggiorna il database
                _storeMetadata.AddFile(IdDocument, sMD5);

            }// fine attivita' per documento in ADD

            return isInDB;
        }
    }
}
