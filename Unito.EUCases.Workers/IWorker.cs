using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace Unito.EUCases.Workers
{
    public interface IWorker<P, R>: INotifyPropertyChanged
        where P : class, INotifyPropertyChanged, new()
        where R : class, INotifyPropertyChanged, new()
    {
        /// <summary>
        /// Parameters used to run worker
        /// </summary>
        P Parameters { get; set; }

        /// <summary>
        /// Results of process execution
        /// </summary>
        R Results { get;  }

        /// <summary>
        /// Status of the worker
        /// </summary>
        WorkerStatus Status { get; }

        /// <summary>
        /// Start processing worker based on current parameters
        /// </summary>
        void DoWork(CancellationToken token);

        /// <summary>
        /// True when worker is in Executing status and user can cancel it 
        /// using Cancel command
        /// </summary>
        bool CanCancel { get; }

        /// <summary>
        /// Reset worker to initial status.
        /// Command available only in executed status
        /// </summary>
        void Reset();

        /// <summary>
        /// Display execution percentage 
        /// </summary>
        float? ExecutionPercentage { get; }
    }
}
