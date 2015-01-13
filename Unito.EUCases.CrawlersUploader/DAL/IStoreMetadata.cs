using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.CrawlersUploader.DAL
{
    public interface IStoreMetadata
    {
        bool AddFile(String IdDocument, String MD5);
        bool ExistsFile(String IdDocument);
        bool CheckFileIsEqual(String IdDocument, String MD5);
        bool UpdateFile(String IdDocument, String MD5);
        bool SaveData();

    }
}
