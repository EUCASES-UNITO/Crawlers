using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Repository.Hierarchy;
using System.Configuration;

namespace Unito.EUCases.log4net
{
    public static class SimpleConfig
    {
        /// <summary>
        /// Dimensione di default per il rolling appender (10MB)
        /// </summary>
        public const string DefaultMaximumFileSize = "10MB";

        /// <summary>
        /// Pattern di default da utilizzare nel log
        /// (%d{dd-MM-yyyy HH:mm:ss} [%t] %-5p %c - %m%n)
        /// </summary>
        public const string DefaultPattern = "%d{dd-MM-yyyy HH:mm:ss} [%t] %-5p %c - %m%n";

        public const string DefaultLogPatternKey = "LogPattern";

        public const string DefaultLogLevelKey = "LogLevel";

        public const string DefaultLogNamespacesKey = "LogNamespaces";

        /// <summary>
        /// Valore di default per il logging a livello generale (Error)
        /// </summary>
        public static readonly Level DefaultLevel = Level.Error;

        /// <summary>
        /// Restituisce un RollingFileAppendere inizializzato con i parametri passati
        /// </summary>
        /// <param name="filename">Nome del file su cui scrivere</param>
        /// <param name="maximumFileSize">Dimensione massima del file, se non specificato viene utilizzato il valore
        /// SimpleConfig.DefaultMaximumFileSize</param>
        /// <param name="pattern">Pattern da utilizzare, se non specificato viene utilizzato il valore 
        /// SimpleConfig.DefaultPattern</param>
        /// <returns>RollingFileAppender configurato e attivato</returns>
        public static RollingFileAppender GetRollingFileAppender(
                string filename,
                string maximumFileSize = DefaultMaximumFileSize,
                string pattern = DefaultPattern
            )
        {
            var patternLayout = new PatternLayout
            {
                ConversionPattern = pattern,
            };
            patternLayout.ActivateOptions();

            var tracer = new RollingFileAppender
            {
                AppendToFile = true,
                File = filename,
                MaximumFileSize = DefaultMaximumFileSize,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                Layout = patternLayout
            };
            tracer.ActivateOptions();
            
            return tracer;
        }        

        /// <summary>
        /// Configura il sistema di logging a partire dalla configurazione del sistema e utilizzando un
        /// rolling file appender
        /// </summary>
        /// <param name="filenameSettingKey">Chiave utilizzata nell'appSettings per il nome del file di log, 
        /// valore di default LogFileName.
        /// Se non viene trovata l'impostazione viene alzata un'eccezione di tipo ArgumentException</param>
        /// <param name="levelSettingKey">Chiave utilizzata nell'appSettings per il livello di log di default, 
        /// valore di default LogLevel.
        /// Se non viene trovata l'impostazione viene utilizzato il valore di default</param>
        /// <param name="logPatternKey">Chiave utilizzata nell'appSettings per il pattern da utilizzare nel logging, 
        /// valore di default LogPattern.
        /// Se non viene trovata l'impostazione viene utilizzato il valore di default</param>
        /// <param name="filesizeSettingKey">Chiave utilizzata nell'appSettings per le dimensioni massime del file di log, 
        /// valore di default LogFileSize
        /// Se non viene trovata l'impostazione viene utilizzato il valore di default</param>
        /// <param name="namespaceLevelsKey">Chiave utilizzata nell'appSettings per l'eventuale elenco di livello di logging da associare ai namespaces, 
        /// valore di default LogNamespaces
        /// Se non viene trovata l'impostazione non vengono impostati valori custom per i namespaces</param>
        /// <param name="filenameTransformation">Eventuale funzione di mapping da applicare al filename,
        /// utile particolarmente in una web application se si vuole utilizzare un percorso relativo (es. ~/app_data/log.txt)</param>
        public static void ConfigureFromSettings(
            string filenameSettingKey = "LogFileName",
            string levelSettingKey = DefaultLogLevelKey,
            string logPatternKey = DefaultLogPatternKey,
            string filesizeSettingKey = "LogFileSize",
            string namespaceLevelsKey = DefaultLogNamespacesKey,
            Func<string, string> filenameTransformation = null)
        {
            
            var filename = ConfigurationManager.AppSettings[filenameSettingKey];
            if ( filename.IsNullOrEmpty() )
                throw new ArgumentException("Filename key not found in configuration or empty");
            if ( filenameTransformation != null )
                filename = filenameTransformation(filename);

            var logPattern = ConfigurationManager.AppSettings[logPatternKey];
            if (logPattern.IsNullOrEmpty())
                logPattern = DefaultPattern;
            
            var logFileSize = ConfigurationManager.AppSettings[filesizeSettingKey];
            if ( logFileSize.IsNullOrEmpty() )
                logFileSize = DefaultMaximumFileSize;

            
            
            var appender = GetRollingFileAppender(filename, logFileSize, logPattern);

            ConfigureFromSettings(appender, levelSettingKey, namespaceLevelsKey);
        }

        public static void ConfigureFromSettings(
            IAppender appender,
            string levelSettingKey = DefaultLogLevelKey,
            string namespaceLevelsKey = DefaultLogNamespacesKey)
        {
             
           
            var level = ConfigurationManager.AppSettings[levelSettingKey];
            Level defaultLevel;
            if (level.IsNullOrEmpty())
            {
                defaultLevel = DefaultLevel;
            }
            else
            {
                defaultLevel = LogManager.GetRepository().LevelMap[level];
            }

            var namespaceLevels = ConfigurationManager.AppSettings[namespaceLevelsKey];

            Configure(appender, defaultLevel, namespaceLevels);
        }

        /// <summary>
        /// Configura log4net con un appender specifico ed eventualmente dei livelli di 
        /// logging specifici
        /// </summary>
        /// <param name="appender">Appendere da utilizzare</param>
        /// <param name="level">Livello di default per il logging, se non specificato o passato 
        /// come null, utilizzato il valore di default Level.Error</param>
        /// <param name="namespaceLevels">Eventuale elenco di coppie di namespaces e valori 
        /// da inizializzare con valori specifici specificati come [namespace],[level];[namespace],[level] ecc.</param>
        public static void Configure(IAppender appender,
            Level level = null,
            string namespaceLevels = null)
        {
            Dictionary<string, Level> namespaceLevelsDictionary = null;
            if (!namespaceLevels.IsNullOrEmpty())
            {
                namespaceLevelsDictionary = new Dictionary<string, Level>();
                foreach (var s in namespaceLevels.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries ))
                {
                    var pair = s.Split(',');
                    if (pair.Length != 2)
                        throw new ArgumentException("Invalid namespaceLevels string");
                    var customLevel = LogManager.GetRepository().LevelMap[pair[1]];
                    namespaceLevelsDictionary.Add(pair[0], customLevel);
                }
            }
            Configure(appender, level, namespaceLevelsDictionary);
        }

        /// <summary>
        /// Configura log4net con un appender specifico ed eventualmente dei livelli di 
        /// logging specifici
        /// </summary>
        /// <param name="appender">Appendere da utilizzare</param>
        /// <param name="level">Livello di default per il logging, se non specificato o passato 
        /// come null, utilizzato il valore di default Level.Error</param>
        /// <param name="namespaceLevels">Eventuale elenco di coppie di namespaces e valori 
        /// da inizializzare con valori specifici</param>
        public static void Configure(IAppender appender,
            Level level = null,
            Dictionary<string, Level> namespaceLevels = null)
        {
            if (level == null)
                level = DefaultLevel;
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.Level = level;
            hierarchy.Root.AddAppender(appender);

            if (!namespaceLevels.IsNullOrEmpty())
            {
                foreach (var pair in namespaceLevels)
                {
                    var log = LogManager.GetLogger(pair.Key);
                    ((Logger)log.Logger).Level = pair.Value;
                }
            }
            hierarchy.Configured = true;    
        }

        public static void AddAppender(IAppender appender)
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            hierarchy.Root.AddAppender(appender);
        }

        public static void ResetCustomLevels(string customLevels)
        {
            var hierarchy = LogManager.GetRepository() as Hierarchy;
            try
            {

                foreach (var logger in hierarchy.GetCurrentLoggers())
                {
                    ((Logger)logger).Level = null;
                }
                foreach (var s in customLevels.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var parts = s.Split(',');
                    var loggerName = parts[0];
                    var logLevel = hierarchy.LevelMap[parts[1]];
                    if (logLevel != null)
                        (hierarchy.GetLogger(loggerName) as Logger).Level = logLevel;

                }
            }            
            catch { }
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
        }
    }
}
