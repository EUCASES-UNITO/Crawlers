using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unito.EUCases.Crawlers.Normattiva;
using Unito.EUCases.CrawlersUploader;
using Unito.EUCases.Email;

namespace Unito.EUCases.Crawlers.NormattivaWorker
{
    public class NormattivaParameters:INotifyPropertyChanged
    {
        private Unito.EUCases.Crawlers.Normattiva.AkomaNtoso.AkomaNtosoParameters _akomaNtosoParameters = new Unito.EUCases.Crawlers.Normattiva.AkomaNtoso.AkomaNtosoParameters();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Unito.EUCases.Crawlers.Normattiva.AkomaNtoso.AkomaNtosoParameters AkomaNtosoParameters
        {
            get { return _akomaNtosoParameters; }
            set { _akomaNtosoParameters = value; OnPropertyChanged("AkomaNtoso Parameters"); }
        }

        private Parameters _crawlerParameters = new Parameters();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Parameters CrawlerParameters
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

        private ParametersEmail _emailParameters = new ParametersEmail();
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ParametersEmail EmailParameters
        {
            get { return _emailParameters; }
            set { _emailParameters = value; OnPropertyChanged("EmailParameters"); }
        }
    }
}
