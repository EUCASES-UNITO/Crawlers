using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Workers
{
    public class ResultBase:INotifyPropertyChanged
    {
        public ResultBase()
        {
            Statistics = new ExecutionStatistics();
        }

        private ExecutionStatistics _statistics;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ExecutionStatistics Statistics
        {
            get { return _statistics; }
            set
            {
                if (_statistics == value) return;
                _statistics = value;
                OnPropertyChanged("Statistics");
            }
        }
        
        private bool? _success;
        public bool? Success
        {
            get { return _success; }
            set {
                if (_success == value) return;
                _success = value; 
                OnPropertyChanged("Success"); 
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
