using System;
using System.Collections.Generic;
using ActiveUp.Net.Mail;
using Library.Core.Messaging.Settings;
using Library.Services.Messaging;

namespace Library.Core.Messaging.Infrastructure
{

    public interface IImapMailReader : IDisposable
    {
        IReadOnlyCollection<Message> GetUnreadMails(string mailBox, int messageCount);
        ImapMailReader CreateConnection(IIMapServerSettings mailConfig);
        bool IsConnected();
        bool HasUnreadMail();
    }
}