using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebsiteService.Services.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpClient m_smtpClient;
        private readonly string m_sender;

        public SmtpEmailService(IEmailServiceSettings settings)
        {
            m_sender = settings.UserName;

            m_smtpClient = new SmtpClient(settings.SmtpUrl);

            m_smtpClient.Port = 587;
            m_smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            m_smtpClient.UseDefaultCredentials = false;
            m_smtpClient.EnableSsl = true;
            m_smtpClient.Credentials = new NetworkCredential(m_sender, settings.Password);
        }

        public void sendEmail(string toAddress, string subject, string body)
        {
            var mail = new MailMessage(m_sender, toAddress.Trim(), subject, body);

            m_smtpClient.Send(mail);
        }
    }
}