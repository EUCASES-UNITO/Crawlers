using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unito.EUCases.Crawlers.GiustiziaAmministrativaWorker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Workers.WorkerProgramHelper<GiustiziaAmministrativaWorker, GiustiziaAmministrativaParameters, GiustiziaAmministativaResults>.MainImpl(args);
        }

    }
}
