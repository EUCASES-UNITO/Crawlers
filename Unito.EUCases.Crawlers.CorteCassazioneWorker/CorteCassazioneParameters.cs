using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.CorteCassazione;
using Unito.EUCases.CrawlersUploader;

namespace Unito.EUCases.Crawlers.CorteCassazioneWorker
{
    public class CorteCassazioneParameters : INotifyPropertyChanged
    {
        private ParametersCorteCassazione _crawlerParameters = new ParametersCorteCassazione();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Unito.EUCases.Crawlers.CorteCassazione.ParametersCorteCassazione CrawlerParameters
        {
            get { return _crawlerParameters; }
            set { _crawlerParameters = value; OnPropertyChanged("CrawlerParameters"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
    }
}
