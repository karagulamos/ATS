using ActiveUp.Net.Mail;
using Library.Core.Messaging.Settings;

namespace Library.Services.Messaging
{
    public class InboxMessageWrapper
    {
        public Message Message { get; set; }
        public IIMapServerSettings ImapSetting { get; set; }
    }
}