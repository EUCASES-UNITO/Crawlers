using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.CorteCostituzionale
{
    public class ParametersCorteCostituzionale
    {
        public int EndYear { get; set; }
        public int StartYear { get; set; }
        

        // url di riferimento per iniziare la ricerca http://www.cortecostituzionale.it/actionPronuncia.do

        /// <summary>
        /// URL della pagina di ricerca da cui trovare l'elenco delle leggi da scaricare
        /// è possibile utilizzare come parametri {0} e {1} i valori StartYear e EndYear
        /// </summary>
        public string URLPronunce { get; set; }
        public string URLMassime { get; set; }

        public int MaxRandomWait { get; set; }
        public string CallNext { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }

       

    }
}
