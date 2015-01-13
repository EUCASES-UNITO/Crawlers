using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.EurLex;

namespace Unito.EUCases.Crawlers.EurLexWorker
{
    public class EurLexParameters : INotifyPropertyChanged
    {
        private ParametersEurLex _crawlerParameters = new ParametersEurLex();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ParametersEurLex CrawlerParameters
        {
            get { return _crawlerParameters; }
            set { _crawlerParameters = value; OnPropertyChanged("CrawlerParameters"); }
        }        

        private string _destinationFolder;
        public string DestinationFolder
        {
            get { return _destinationFolder; }
            set { _destinationFolder = value; OnPropertyChanged("DestinationFolder"); }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
