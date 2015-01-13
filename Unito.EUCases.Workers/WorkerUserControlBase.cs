using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using l4n = log4net;
using log4net.Core;

namespace Unito.EUCases.Workers
{
    public partial class WorkerUserControlBase : UserControl
        
    {
        protected l4n.ILog _log;

        // TODO Results property Grid in read-only
        // TODO Parameters property grid in read-only in stato diverso da WaitForStart

        string _currentCustomLogLevels;

        public WorkerUserControlBase()
        {
            InitializeComponent();
            mergeToolstrip(parametersPropertyGrid, parametersToolStrip);
            mergeToolstrip(resultsPropertyGrid, resultsToolStrip);
            _log = l4n.LogManager.GetLogger(this.GetType());

            logLevelDropDownList.Items.Add(Level.Error);
            logLevelDropDownList.Items.Add(Level.Info);
            logLevelDropDownList.Items.Add(CustomLevels.Detail);
            logLevelDropDownList.Items.Add(Level.Debug);
            logLevelDropDownList.Items.Add(Level.Verbose);
            var loggerRepository = l4n.LogManager.GetRepository() as l4n.Repository.Hierarchy.Hierarchy;
            
            if (!logLevelDropDownList.Items.Contains(loggerRepository.Root.Level))
            {
                logLevelDropDownList.Items.Add(loggerRepository.Root.Level);
                
            }
            var current = logLevelDropDownList.Items.Cast<Level>().SingleOrDefault(_ => _.Name == ProgramLaunchOptions.Current.LogLevel);
            if (current != null)
                logLevelDropDownList.SelectedItem = current;

            

            _currentCustomLogLevels = ProgramLaunchOptions.Current.CustomLogLevels;
            customLogLevelsTextBox.Text = _currentCustomLogLevels;
        }

        void mergeToolstrip(PropertyGrid propertyGrid, ToolStrip sourceToolstrip)
        {
            var propertyGridToolstrip = propertyGrid.GetType().InvokeMember("toolStrip", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance, null, propertyGrid, null)
                as ToolStrip;
            var elements = sourceToolstrip.Items.Cast<ToolStripItem>().ToList();
            foreach (var element in elements)
            {
                sourceToolstrip.Items.Remove(element);
                propertyGridToolstrip.Items.Add(element);
            }
            sourceToolstrip.Visible = false;
        }

        private void clearTraceLogButton_Click(object sender, EventArgs e)
        {
            traceLogTextBox.Text = string.Empty;
        }

        private void logLevelDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var loggerRepository = l4n.LogManager.GetRepository() as l4n.Repository.Hierarchy.Hierarchy;
            loggerRepository.Root.Level = logLevelDropDownList.SelectedItem as Level;
            loggerRepository.RaiseConfigurationChanged(EventArgs.Empty);
        }

        private void customLogLevelsTextBox_Leave(object sender, EventArgs e)
        {
            if (_currentCustomLogLevels == customLogLevelsTextBox.Text)
                return;
            _currentCustomLogLevels = customLogLevelsTextBox.Text;
            Unito.EUCases.log4net.SimpleConfig.ResetCustomLevels(_currentCustomLogLevels);
        }
    }

    
}
