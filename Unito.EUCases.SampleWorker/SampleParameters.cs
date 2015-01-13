using CommandLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.SampleWorker
{
    public class SampleParameters : INotifyPropertyChanged
    {
        private string _message;
        [Option('m', "message", HelpText = "String to print during worker exeuction")]        
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }


        private int _duration;
        [Option('w', "wait", HelpText = "Time to wait befor worker end")]
        public int Duration
        {
            get { return _duration; }
            set { _duration = value; OnPropertyChanged("Duration"); }
        }
        

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
