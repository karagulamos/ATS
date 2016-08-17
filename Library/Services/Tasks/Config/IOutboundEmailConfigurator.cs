using System.Net.Mail;
using Library.Core.Persistence.Repositories;

namespace Library.Services.Tasks.Config
{
    public interface IOutboundEmailConfigurator
    {
        SmtpClient ConfigureSmtpClient(OutboundEmailDetail email);
        MailAddress ConfigureMailAddress(OutboundEmailDetail email);
    }
}