using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unito.EUCases.log4net
{
    // http://stackoverflow.com/questions/14114614/configuring-log4net-textboxappender-custom-appender-via-xml-file
    // http://triagile.blogspot.co.uk/2010/12/log4net-appender-for-displaying.html
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class TextBoxAppender:AppenderSkeleton
    {
        private TextBox _textBox;
        private readonly object _lockObj = new object();

        Task _asyncWrite;

        public TextBoxAppender(TextBox textBox)
        {
            var frm = textBox.FindForm();
            
            _textBox = textBox;
            Name = "TextBoxAppender";
            _asyncWriteCancellation = new CancellationTokenSource();
            _asyncWriteCancellationToken = _asyncWriteCancellation.Token;
            _asyncWrite = new Task(AsyncWrite);
            _asyncWrite.Start();
        }


        protected override void OnClose()
        {
            lock (_lockObj)
            {
                _textBox = null;
                _asyncWriteCancellation.Cancel();
            }
            base.OnClose();
        }

        string _pendingText = string.Empty;
        protected override void Append(global::log4net.Core.LoggingEvent loggingEvent)
        {
            lock (_lockObj)
            {
                if (_textBox == null )
                    return;
                var form = _textBox.FindForm();
                if (form != null)
                {
                    form.FormClosing += (s, e) => _textBox = null;
                }
                var msg = string.Concat(loggingEvent.RenderedMessage, Environment.NewLine);                
                if ( Layout != null )
                {
                    using ( var sw = new System.IO.StringWriter() )
                    {
                        Layout.Format(sw, loggingEvent);
                        msg = sw.ToString();
                    }
                }
                if (loggingEvent.ExceptionObject != null)
                    msg = string.Concat(msg, loggingEvent.ExceptionObject.ToString(), Environment.NewLine);

                // viene utilizzato un buffer in quanto se il flusso di log è troppo elevato
                // la scrittura continua sulla textbox blocca l'esecuzione
                _pendingText += msg;                
            }
        }

        DateTime _lastUpdate = DateTime.MinValue;
        CancellationTokenSource _asyncWriteCancellation;
        CancellationToken _asyncWriteCancellationToken;
        void AsyncWrite()
        {
            _asyncWriteCancellationToken.ThrowIfCancellationRequested();
            try
            {
                while (true)
                {

                    var millisecondsToWait = 500 - (int)(DateTime.Now - _lastUpdate).TotalMilliseconds;
                    if (millisecondsToWait > 0)
                        Thread.Sleep(millisecondsToWait);

                    lock (_lockObj)
                    {
                        if (_pendingText.IsNullOrEmpty())
                            continue;
                        var text = _pendingText;
                        var del = new Action<string>((s) => _textBox.AppendText(text));
                        _textBox.BeginInvoke(del, text);
                        _pendingText = string.Empty;
                        _lastUpdate = DateTime.Now;
                    }
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}
