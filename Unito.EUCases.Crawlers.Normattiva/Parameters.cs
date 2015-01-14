using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.Normattiva
{
    public class Parameters:INotifyPropertyChanged
    {
        public Parameters() {
            URL = "http://www.normattiva.it/ricerca/avanzata/aggiornamenti";
        }

        public string URL { get; set; } //DANIELE 17/04/2014

        private int _startMonth = DateTime.Today.Month;
        public int StartMonth
        {
            get { return _startMonth; }
            set
            {
                if (int.Equals(_startMonth, value))
                    return;
                _startMonth = value;
                OnPropertyChanged("StartMonth"); 
            }
        }

        private int _startYear = DateTime.Today.Year;
        public int StartYear
        {
            get { return _startYear; }
            set
            {
                if (int.Equals(_startYear, value))
                    return;
                _startYear = value;
                OnPropertyChanged("StartYear");
            }
        }

        private int _endMonth = DateTime.Today.Month;
        public int EndMonth
        {
            get { return _endMonth; }
            set
            {
                if (int.Equals(_endMonth, value))
                    return;
                _endMonth = value;
                OnPropertyChanged("EndMonth");
            }
        }

        private int _endYear = DateTime.Today.Year;
        public int EndYear
        {
            get { return _endYear; }
            set
            {
                if (int.Equals(_endYear, value))
                    return;
                _endYear = value;
                OnPropertyChanged("EndYear");
            }
        }

        public int MaxRandomWait { get; set; }

        public string CallNext { get; set; }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        
    }
}
