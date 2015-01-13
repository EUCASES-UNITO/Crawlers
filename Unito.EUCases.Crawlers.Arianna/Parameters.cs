using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Unito.Eucases.Crawlers.Arianna
{
    public class Parameters
    {
        public int StartYear { get; set; }

        public int EndYear { get; set; }

        // es. http://arianna.consiglioregionale.piemonte.it/ariaint/jsp/ricerca/IndiceTesti.jsp?TESTORICERCADEF=&TIPODOCUMENTO=LEGGI&RANGELEGGE=0&RANGEPDL=0&ANNOEMA1={0}&ANNOEMA2={1}&CONTESTORICERCA=documento
        /// <summary>
        /// URL della pagina di ricerca da cui trovare l'elenco delle leggi da scaricare
        /// è possibile utilizzare come parametri {0} e {1} i valori StartYear e EndYear
        /// </summary>
        public string URL { get; set; }
    }
}
