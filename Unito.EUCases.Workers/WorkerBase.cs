using System.Threading;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Workers
{
    public abstract class WorkerBase<P,R>:IWorker<P,R>, INotifyPropertyChanged
        where P : class, INotifyPropertyChanged, new()
        where R : ResultBase, new()
    {
        protected ILog _log;
        
        public WorkerBase()
            : this(new P())
        {

        }

        public WorkerBase(P parameters)
        {
            _parameters = parameters;
            _status = WorkerStatus.WaitToStart;
            _log = LogManager.GetLogger(this.GetType());
        }


        P _parameters = null;
        public P Parameters 
        {
            get
            {
                return _parameters;
            }
            set
            {
                if ((Status & WorkerStatus.WaitToStart) != WorkerStatus.WaitToStart)
                    throw new InvalidOperationException("Parameters can be set only in WaitToStart status");
                if (_parameters == value)
                    return;
                _parameters = value;
                OnPropertyChanged("Parameters");
            }
        }

        R _results = null;
        [ReadOnly(true)]
        public R Results
        {
            get { return _results; }
            protected set
            {
                if (_results == value)
                    return;
                _results = value;
                OnPropertyChanged("Results");
            }
        }

        WorkerStatus _status;
        public WorkerStatus Status
        {
            get { return _status; }
            protected set {
                if (_status == value) return;
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public void DoWork(CancellationToken token)
        {
            if ((Status & WorkerStatus.WaitToStart) != WorkerStatus.WaitToStart)
                throw new NotSupportedException("Can't start a worker that is already running or executed");

            Results = new R {Statistics = {StartTime = DateTime.Now}};
            Status = WorkerStatus.Executing;
            ExecutionPercentage = 0;
            _log.Info("Starting worker");
            try
            {                
                doWorkImplementation(token);
                if ( ! Results.Success.HasValue  )
                    Results.Success = true;
                ExecutionPercentage = 100;

            }
            catch (Exception ex)
            {
                Results.Success = false;
                _log.Error("Unexpected exception during worker execution", ex);
            }
            finally
            {
                Results.Statistics.EndTime = DateTime.Now;
                Status = token.IsCancellationRequested ? WorkerStatus.Cancelled : WorkerStatus.Executed;
            }
            _log.Info(Results.Success.Value ? "Worker executed successfully" : "Worker executed with errors");
        }

        protected abstract void doWorkImplementation(CancellationToken token);

        bool _canCancel;
        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                if (_canCancel == value) return;
                _canCancel = value;
                OnPropertyChanged("CanCancel");
            }
        }

        private float? _executionPercentage;
        public float? ExecutionPercentage
        {
            get { return _executionPercentage; }
            protected set
            {
                if (_executionPercentage == value) return;
                if (value > 100)
                    _executionPercentage = 100;
                else if (value < 0)
                    _executionPercentage = 0;
                else
                    _executionPercentage = value;
                OnPropertyChanged("ExecutionPercentage");
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public void Reset()
        {
            if (!( Status.HasFlag(WorkerStatus.Executed) || Status.HasFlag(WorkerStatus.Cancelled)))
                throw new NotSupportedException("Reset can be called only in Executed status");
            Results = null;
            ExecutionPercentage = null;
            Status = WorkerStatus.WaitToStart;
        }
    }
}
