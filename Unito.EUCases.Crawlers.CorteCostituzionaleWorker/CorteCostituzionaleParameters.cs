using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.CorteCostituzionale;
using Unito.EUCases.CrawlersUploader;
using Unito.EUCases.Email;


namespace Unito.EUCases.Crawlers.CorteCostituzionaleWorker
{
    public class CorteCostituzionaleParameters:INotifyPropertyChanged
    {

        private ParametersEmail _emailParameters = new ParametersEmail();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ParametersEmail EmailParameters
        {
            get { return _emailParameters; }
            set { _emailParameters = value; OnPropertyChanged("EmailParameters"); }
        }

        private ParametersCorteCostituzionale _crawlerParameters = new ParametersCorteCostituzionale();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Unito.EUCases.Crawlers.CorteCostituzionale.ParametersCorteCostituzionale CrawlerParameters
        {
            get { return _crawlerParameters; }
            set { _crawlerParameters = value; OnPropertyChanged("CrawlerParameters"); }
        }

        private ParametersUploader _uploaderParameters = new ParametersUploader();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ParametersUploader UploaderParameters
        {
            get { return _uploaderParameters; }
            set { _uploaderParameters = value; OnPropertyChanged("UploaderParameters"); }
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
