using ActiveUp.Net.Mail;
using Library.Core.Messaging.Settings;

namespace Library.Core.Messaging.Infrastructure
{
    public class InboxMessageWrapper
    {
        public Message Message { get; set; }
        public IIMapServerSettings ImapSetting { get; set; }
    }
}