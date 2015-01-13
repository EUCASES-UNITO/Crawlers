using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.EurLex.Utility.Worker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Workers.WorkerProgramHelper<UtilityWorker, UtilityParameters, UtilityResults>.MainImpl(args);
        }
    }
}
