using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.Eucases.Crawlers
{
    public interface IDownloadItem
    {
        /// <summary>
        /// URL: contiene il percorso a cui scricare il documento XML
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// NavigationPath: contiene l'insieme di tutti gli indirizzi percorsi per reperire il documento
        /// </summary>
        IList <string> NavigationPath { get; set; }

        /// <summary>
        /// Id: contiene l'identificativo univoco del documento
        /// </summary>
        string Id { get; set; }

        ///<summary>
        /// PostValues contiene i valori da eseguire in post per il recupero dei dati
        ///</summary>
        IList<KeyValuePair<String, String>> PostValues
        { get; set; }

    }
}
