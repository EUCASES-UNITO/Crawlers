﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Crawlers.CorteCassazioneWorker
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Workers.WorkerProgramHelper<CorteCassazioneWorker, CorteCassazioneParameters, CorteCassazioneResult>.MainImpl(args);
        }
    }
}
