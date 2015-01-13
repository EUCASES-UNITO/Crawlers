using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Unito.EUCases.CrawlersUploader.DAL
{
    public class StoreMetadaOnFile:IStoreMetadata
    {
        string _pathFileDatabase;
        const string  FileDatabaseName = "CrawlerDatabase.xml";
        documentgroup _docGroup;
        List<documentgroupDocument> _documents ;
        
        public StoreMetadaOnFile(string pathFileDatabase)
        {
            // devo deserializzare il db

            if (!Directory.Exists(pathFileDatabase)) Directory.CreateDirectory(pathFileDatabase);
            _pathFileDatabase = pathFileDatabase;
            // devo caricare il database e se non esiste generare istanza in memoria
            if (File.Exists(_pathFileDatabase + "\\" + FileDatabaseName))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(documentgroup));
                //StringReader sRAdd = new StringReader(_pathFileDatabase + "\\" + FileDatabaseName);
                FileStream ReadFileStream = new FileStream(_pathFileDatabase + "\\" + FileDatabaseName, FileMode.Open, FileAccess.Read, FileShare.Read);
                _docGroup = (documentgroup)serializer.Deserialize(ReadFileStream);
                _documents = _docGroup.document.OfType<documentgroupDocument>().ToList();
            } else
            { 
                _docGroup = new documentgroup();            
                _documents = new List<documentgroupDocument>();
            }
        }

        public bool AddFile(string IdDocument, string MD5)
        {
            documentgroupDocument doc = new documentgroupDocument();
            doc.md5 = MD5;
            doc.file = IdDocument;
            _documents.Add(doc);
            return true;
        }

        public bool ExistsFile(string IdDocument)
        {
            bool found = false;      
            var doc = _documents.Where(a => a.file == IdDocument);
            if (doc.Count() > 0) { found = true; };            
            return found;
        }

        public bool CheckFileIsEqual(string IdDocument, string MD5)
        {
            bool isEqual = false;
            try
            {

                documentgroupDocument doc = (documentgroupDocument)_documents.Where(a => a.file == IdDocument).First();
                if (doc.md5 == MD5) { isEqual = true; } else { isEqual = false; };
                
            }
            catch
            {
                return false;
            }
            return isEqual;

        }

        public bool UpdateFile(string IdDocument, string MD5)
        {

            var listDoc = _documents.Where(a => a.file == IdDocument);
            if (listDoc.Count() > 0)
            {
                documentgroupDocument doc = (documentgroupDocument)listDoc.First();
                doc.md5 = MD5;
            }              
            return true;
        }

        public bool SaveData()
        {
            _docGroup.document = _documents.ToArray();
            XmlSerializer serializer = new XmlSerializer(typeof(documentgroup));
            using (StreamWriter swAdd = new StreamWriter(_pathFileDatabase + "\\" + FileDatabaseName))
            { 
                serializer.Serialize(swAdd, _docGroup);
                swAdd.Close();
            }
            //_pathFileDatabase
            return true;
        }
    }
}
