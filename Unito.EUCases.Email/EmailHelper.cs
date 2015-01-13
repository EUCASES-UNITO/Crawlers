using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unito.EUCases.Email
{
    public class EmailHelper
    {
        public void SendEmail(string subject, string mailMessage, ParametersEmail emailParameters)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.To.Add(emailParameters.DestinationEmail);
            message.Subject = string.Concat("[Crawler]: ", subject);
            message.From = new System.Net.Mail.MailAddress(emailParameters.SenderEmail);
            message.Body = mailMessage;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(emailParameters.SmtpHost);
            smtp.Send(message);
        }
    }
}