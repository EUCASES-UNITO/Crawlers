using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.Normattiva.AkomaNtoso;

namespace Unito.EUCases.Crawlers.NormattivaWorker
{
    class AkomaNtosoParameters : INotifyPropertyChanged
    {
        private AkomaNtosoParameters _crawlerParameters = new AkomaNtosoParameters();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public AkomaNtosoParameters CrawlerParameters
        {
            get { return _crawlerParameters; }
            set { _crawlerParameters = value; OnPropertyChanged("CrawlerParameters"); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    } 
}
