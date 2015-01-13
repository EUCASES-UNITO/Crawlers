using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using l4n = log4net;

namespace Unito.EUCases.Workers
{
    public class ProgramLaunchOptions
    {

        public static ProgramLaunchOptions Current { get; internal set; }

        public ProgramLaunchOptions()
        {
            LogLevel = "INFO";
        }

        [Option('?', "help", HelpText = "Show help for current worker")]
        public bool ShowHelp { get; set; }

        [Option("app-config", Required=false, HelpText="Path of alternative configuration file to use during execution")]
        public string AppConfigFile { get; set; }

        [Option('s', "silent", DefaultValue = false, HelpText = "Execute the worker without user interface")]
        public bool SilentMode { get; set; }

        [Option('p', "parameters", Required = false, HelpText = "Filename containing default parameters for worker execution")]
        public string ParametersFile { get; set; }

        [Option('l', "log-file", HelpText = "Filename for logging (used only in silent mode), start time can be used in filename using parameters {0}")]
        public string LogFile { get; set; }

        [Option("log-level", HelpText="Log level to use during execution (available values DEBUG, INFO, VERBOSE, ERROR)")]
        public string LogLevel { get; set; }

        [Option("custom-log-levels", HelpText = "Log level to use for specific namespaces, formato da utilizzare namespace1,DEBUG;namespace2,INFO")]
        public string CustomLogLevels { get; set; }
    }
}
