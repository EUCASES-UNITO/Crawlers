using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Email
{
    public class ParametersEmail
    {
        private string _SenderEmail;

        public string SenderEmail
        {
            get { return _SenderEmail; }
            set { _SenderEmail = value; }
        }

        private string _DestinationEmail;

        public string DestinationEmail
        {
            get { return _DestinationEmail; }
            set { _DestinationEmail = value; }
        }

        private string smtpHost;

        public string SmtpHost
        {
            get { return smtpHost; }
            set { smtpHost = value; }
        }
        public override string ToString()
        {
            return string.Empty;
        }
    }
}
