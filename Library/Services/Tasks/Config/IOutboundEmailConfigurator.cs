using System.Net.Mail;
using Library.Data;

namespace Library.Services.Tasks.Config
{
    public interface IOutboundEmailConfigurator
    {
        SmtpClient ConfigureSmtpClient(OutboundEmailDetail email);
        MailAddress ConfigureMailAddress(OutboundEmailDetail email);
    }
}