using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Workers
{
    [Flags]
    public enum WorkerStatus
    {
        WaitToStart = 1,
        Executing = 2,
        Executed = 4,
        Cancelled = 16
    }
}
