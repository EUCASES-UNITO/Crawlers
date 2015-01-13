using System.Threading;
using Unito.EUCases.log4net;
using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using l4n = log4net;

namespace Unito.EUCases.Workers
{
    public static class WorkerProgramHelper<W,P,R>
        where W : IWorker<P, R>, new()
        where P : class, INotifyPropertyChanged, new()
        where R : class, INotifyPropertyChanged, new()
    {
        static XmlSerializer _parametersSerializer = new XmlSerializer(typeof(P));
        static XmlSerializer _resultsSerializer = new XmlSerializer(typeof(R));        

        public static string GetHelpText()
        {
            var sb = new StringBuilder();
            
            var help = new HelpText
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true,
            };
            help.AddOptions(new ProgramLaunchOptions());
            sb.AppendLine("Executable parameters")
                .AppendLine(help.ToString());
            
            help.AddOptions(new P());
            sb.AppendLine("Worker specific parameters")
                .AppendLine(help.ToString());
            return sb.ToString();
        }

        public static void SaveParameters(P parameters, string filename) {
            using (var sw = new StreamWriter(filename))
            {
                _parametersSerializer.Serialize(sw, parameters);
            }
        }

        public static void SaveResults(R results, string filename)
        {
            using (var sw = new StreamWriter(filename))
            {
                _resultsSerializer.Serialize(sw, results);
            }
        }

        public static P OpenParameters(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var result = _parametersSerializer.Deserialize(sr) as P;
                return result;
            }
        }

        
        public static void MainImpl(string[] args)
        {
            ProgramLaunchOptions.Current = new ProgramLaunchOptions();            
            CommandLine.Parser.Default.ParseArguments(args, ProgramLaunchOptions.Current);
            
            if ( ! ProgramLaunchOptions.Current.AppConfigFile.IsNullOrEmpty() )
            {
                // Impostazione di un percorso alternativo per l'app.config
                // http://weblogs.asp.net/israelio/archive/2005/01/10/349825.aspx
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", ProgramLaunchOptions.Current.AppConfigFile);
            }
            if (ProgramLaunchOptions.Current.LogLevel.IsNullOrEmpty())
            {
                ProgramLaunchOptions.Current.LogLevel = ConfigurationManager.AppSettings["DefaultLogLevel"];
            }
            if (ProgramLaunchOptions.Current.CustomLogLevels.IsNullOrEmpty())
            {
                ProgramLaunchOptions.Current.CustomLogLevels = ConfigurationManager.AppSettings["DefaultCustomLogLevels"];
            }

            if (ProgramLaunchOptions.Current.ShowHelp)
            {
                Console.WriteLine(GetHelpText());                
                return;
            }

            W worker = new W();
            
            if (!ProgramLaunchOptions.Current.ParametersFile.IsNullOrEmpty())
                worker.Parameters = OpenParameters(ProgramLaunchOptions.Current.ParametersFile);
            else
                worker.Parameters = new P();
            
            if (!CommandLine.Parser.Default.ParseArguments(args, worker.Parameters))
            {
                // show usage
            }
            if (ProgramLaunchOptions.Current.SilentMode)
                ExecuteSilent(worker);
            else
                ExecuteUI(worker);
        }

        static void ExecuteSilent(W worker)
        {
            SetLoggerForSilentMode();

            var tokenSource = new CancellationTokenSource();

            var task = new Task(() => worker.DoWork(tokenSource.Token));
            task.Start();

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                if (worker.CanCancel)
                {
                    tokenSource.Cancel();
                    eventArgs.Cancel = true;
                }
            };

            task.Wait();
        }

        static void SetLoggerForSilentMode()
        {
            l4n.Core.CustomLevels.SetUp();
            var consoleAppender = new l4n.Appender.ConsoleAppender
            {
                Layout = getLogPattern()
            };
            consoleAppender.ActivateOptions();
            var level = l4n.LogManager.GetRepository().LevelMap[ProgramLaunchOptions.Current.LogLevel];
            Unito.EUCases.log4net.SimpleConfig.Configure(consoleAppender, level, ProgramLaunchOptions.Current.CustomLogLevels);
            if (!ProgramLaunchOptions.Current.LogFile.IsNullOrEmpty())
            {
                var filename = string.Format(ProgramLaunchOptions.Current.LogFile, DateTime.Now);
                Unito.EUCases.log4net.SimpleConfig.AddAppender(
                    Unito.EUCases.log4net.SimpleConfig.GetRollingFileAppender(filename));
            }            
        }

        private static l4n.Layout.PatternLayout getLogPattern()
        {
            var patternLayout = new l4n.Layout.PatternLayout
            {
                ConversionPattern = "%d{HH:mm:ss} [%t] %-5p - %m%n",
            };
            patternLayout.ActivateOptions();
            return patternLayout;
        }

        static void ExecuteUI(W worker)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            var frm = new WorkerForm<W, P, R>(worker);
            SetLoggerForUI(frm);
            System.Windows.Forms.Application.Run(frm);
        }

        static void SetLoggerForUI(WorkerForm<W, P, R> frm)
        {
            l4n.Core.CustomLevels.SetUp();
            var appender = new TextBoxAppender(frm.workerUserControl.traceLogTextBox);
            appender.Layout = getLogPattern();
            appender.ActivateOptions();
            var level = l4n.LogManager.GetRepository().LevelMap[ProgramLaunchOptions.Current.LogLevel];
            Unito.EUCases.log4net.SimpleConfig.Configure(appender, level, ProgramLaunchOptions.Current.CustomLogLevels);
        }
    }
}
