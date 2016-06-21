using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Library.Data;

namespace Library.Services.Tasks.Config
{
    [Export(typeof(IOutboundEmailConfigurator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class OutboundEmailConfigurator : IOutboundEmailConfigurator
    {
        private readonly string _sender = ConfigurationManager.AppSettings["Sender"];
        private readonly string _password = ConfigurationManager.AppSettings["Password"];
        private readonly string _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        private readonly int _serverPort = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        private readonly bool _enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

        public SmtpClient ConfigureSmtpClient(OutboundEmailDetail email)
        {
            return new SmtpClient(_smtpServer)
            {
                Port = _serverPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_sender, _password),
                EnableSsl = _enableSsl
            };
        }

        public MailAddress ConfigureMailAddress(OutboundEmailDetail email)
        {
            return new MailAddress(_sender, email.Sender);
        }

       
    }
}