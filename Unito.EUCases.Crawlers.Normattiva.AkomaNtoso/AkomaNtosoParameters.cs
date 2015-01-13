using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.Normattiva.AkomaNtoso
{
    public class AkomaNtosoParameters : INotifyPropertyChanged
    {

        
        private bool _useAkomaNtosoService;

        public bool UseAkomaNtosoService
        {
            get { return _useAkomaNtosoService; }
            set { _useAkomaNtosoService = value;
            OnPropertyChanged("UseAkomaNtosoService");
            }
        }
        


        private string _AkomServicePath;
        public string AkomaServicePath
        {
            get { return _AkomServicePath; }
            set
            {
                if (string.Equals(_AkomServicePath, value))
                    return;
                _AkomServicePath = value;
                OnPropertyChanged("Akoma Service Path");
            }
        }


        private string _DocumentsSourcePath;
        public string DocumentsSourcePath
        {
            get { return _DocumentsSourcePath; }
            set
            {
                if (string.Equals(_DocumentsSourcePath, value))
                    return;
                _DocumentsSourcePath = value;
                OnPropertyChanged("Documents Source Path");
            }
        }


        private string _DocumentsDestinationPath;
        public string DocumentsDestinationPath
        {
            get { return _DocumentsDestinationPath; }
            set
            {
                if (string.Equals(_DocumentsDestinationPath, value))
                    return;
                _DocumentsDestinationPath = value;
                OnPropertyChanged("Documents Source Path");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
