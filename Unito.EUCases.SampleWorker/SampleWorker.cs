using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Unito.EUCases.SampleWorker
{
    public class SampleWorker:Workers.WorkerBase<SampleParameters, Workers.ResultBase>
    {       
        protected override void doWorkImplementation(System.Threading.CancellationToken token)
        {
            base._log.Info(Parameters.Message);
            Thread.Sleep(Parameters.Duration);
            Results.Success = true;
        }
    }
}
