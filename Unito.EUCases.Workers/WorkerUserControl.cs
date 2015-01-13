using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unito.EUCases.Workers
{
    public class WorkerUserControl<W,P,R>:WorkerUserControlBase
        where W : IWorker<P,R>, new()
        where P : class, INotifyPropertyChanged, new()
        where R : class, INotifyPropertyChanged, new()
    {                
        public WorkerUserControl()
        {
            startButton.Click += startButton_Click;
            cancelButton.Click += cancelButton_Click;
            resetButton.Click += resetButton_Click;
            parametersOpenButton.Click += parametersOpenButton_Click;
            parametersSaveButton.Click += parametersSaveButton_Click;
            resultsSaveButton.Click += resultsSaveButton_Click;
            saveLogButton.Click += saveLogButton_Click;
            usageTextBox.Text = WorkerProgramHelper<W, P, R>.GetHelpText();
            
        }

        void saveLogButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "LOG files (*.log)|*.log|TXT files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, traceLogTextBox.Text);
                MessageBox.Show("Trace saved successfully");
            }
        }

        void resultsSaveButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    WorkerProgramHelper<W, P, R>.SaveResults(_worker.Results, dialog.FileName);
                    MessageBox.Show("Results saved successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
            }
        }

        void parametersSaveButton_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog { 
                AddExtension = true, 
                CheckPathExists = true,                 
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    WorkerProgramHelper<W, P, R>.SaveParameters(_worker.Parameters, dialog.FileName);
                    MessageBox.Show("Parameters saved successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
            }
        }

        void parametersOpenButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _worker.Parameters = WorkerProgramHelper<W, P, R>.OpenParameters(dialog.FileName);
                    parametersPropertyGrid.SelectedObject = _worker.Parameters;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unexpected error: " + ex.Message);
                }
            }
        }

        void resetButton_Click(object sender, EventArgs e)
        {
            _worker.Reset();
            initialize();
            parametersAndResultsTabControl.SelectedIndex = 0;           
        }

        void cancelButton_Click(object sender, EventArgs e)
        {
            _tokenSource.Cancel();
        }

        protected CancellationTokenSource _tokenSource;
        void startButton_Click(object sender, EventArgs e)
        {
            _tokenSource = new CancellationTokenSource();
            var task = new Task(() => _worker.DoWork(_tokenSource.Token));
            task.Start();
        }

        W _worker;

        public void SetWorker(W worker)
        {
            if (_worker != null)
            {
                _worker.PropertyChanged -= worker_PropertyChanged;
            }
            _worker = worker;
            initialize();
            _worker.PropertyChanged += worker_PropertyChanged;
        }

        void initialize()
        {           
            setParametersBinding();
            setLogPanel();
            setCancelButton();
            setResetButton();
            setResultsTab();
            setResultsBinding();
            setPercentage();
            setStatus();
            setParametersButtons();
        }

        private void worker_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            if ( this.InvokeRequired )
            {
                
                this.BeginInvoke((Action)(() => onPropertyChanged(propertyName)));
            }
            else
            {
                onPropertyChanged(propertyName);
            }            
        }

        void onPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "Parameters":
                    setParametersBinding();
                    break;
                case "Status":
                    setLogPanel();
                    setCancelButton();
                    setResetButton();
                    setResultsTab();
                    setPercentage();
                    setStatus();
                    setParametersButtons();
                    break;
                case "CanCancel":
                    setCancelButton();
                    break;
                case "Results":
                    setResultsBinding();
                    break;
                case "ExecutionPercentage":
                    setPercentage();
                    break;
            }
        }

        void setParametersBinding()
        {
            parametersPropertyGrid.SelectedObject = _worker.Parameters;
            _worker.Parameters.PropertyChanged += (source, args) =>
            {
                parametersPropertyGrid.Refresh();
            };
        }

        void setResultsBinding()
        {
            resultsPropertyGrid.SelectedObject = _worker.Results;
        }

        void setParametersButtons()
        {
            parametersOpenButton.Enabled = _worker.Status.HasFlag(WorkerStatus.WaitToStart);            
        }

        
        void setLogPanel()
        {
            var tabPages = parametersAndResultsTabControl.TabPages;
            /* if (_worker.Status.HasFlag(WorkerStatus.WaitToStart) && traceLogTextBox.Text.IsNullOrEmpty())
            {
                if ( tabPages.Contains(logTabPage) )
                    tabPages.Remove(logTabPage);                    
            }
            else
            { 
                if (!tabPages.Contains(logTabPage))
                {
                    tabPages.Insert(1, logTabPage);
                    parametersAndResultsTabControl.SelectedIndex = 1;
                }
            } */
            if (!tabPages.Contains(logTabPage))
            {
                tabPages.Insert(1, logTabPage);
                parametersAndResultsTabControl.SelectedIndex = 1;
            }            
        }

        void setCancelButton()
        {
            cancelButton.Enabled = _worker.Status.HasFlag(WorkerStatus.Executing) &&
                                   _worker.CanCancel &&
                                   ! (_tokenSource != null && _tokenSource.Token.IsCancellationRequested);                
        }

        void setResetButton()
        {
            resetButton.Enabled = _worker.Status.HasFlag(WorkerStatus.Executed) || _worker.Status.HasFlag(WorkerStatus.Cancelled);
        }

        void setResultsTab()
        {            
            if (_worker.Status.HasFlag(WorkerStatus.Executed) || _worker.Status.HasFlag(WorkerStatus.Cancelled) )
            {
                if (!parametersAndResultsTabControl.TabPages.Contains(resultsTabPage))
                {
                    parametersAndResultsTabControl.TabPages.Insert(2, resultsTabPage);
                    parametersAndResultsTabControl.SelectedIndex = 2;
                }
            }
            else
            {
                if (parametersAndResultsTabControl.TabPages.Contains(resultsTabPage))
                    parametersAndResultsTabControl.TabPages.Remove(resultsTabPage);
            }
        }

        void setStatus()
        {
            toolStripStatusLabel.Text = string.Format("{0}", _worker.Status);
            startButton.Enabled = _worker.Status.HasFlag(WorkerStatus.WaitToStart);
        }

        void setPercentage()
        {
            if ( _worker.Status.HasFlag(WorkerStatus.Executing) )
            {
                toolStripProgressBar.Visible = true;
                toolStripProgressBar.Value = (int)_worker.ExecutionPercentage.GetValueOrDefault(0);
            }
            else
            {
                toolStripProgressBar.Visible = false;
            }
        }
    }
}
