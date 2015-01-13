using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Workers
{
    public class ExecutionStatistics:INotifyPropertyChanged
    {
        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                if (_startTime == value) return;
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }
        


        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set {
                if (_endTime == value) return;
                _endTime = value; OnPropertyChanged("EndTime"); }
        }
        
        public virtual TimeSpan? Duration
        {
            get
            {
                if (StartTime.HasValue && EndTime.HasValue)
                    return EndTime.Value - StartTime.Value;
                else
                    return null;
            }
        }

        public override string ToString()
        {
            var duration = Duration;
            if (duration.HasValue)
                return string.Format("Durantion {0}", duration);
            else
                return string.Empty;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                switch (propertyName)
                {
                    case "EndTime":
                    case "StartTime":
                        OnPropertyChanged("Duration");
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
