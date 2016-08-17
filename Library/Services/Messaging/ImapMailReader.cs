using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using ActiveUp.Net.Mail;
using Library.Core.Messaging.Infrastructure;
using Library.Core.Messaging.Settings;

namespace Library.Services.Messaging
{
    [Export(typeof(IImapMailReader))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ImapMailReader : IImapMailReader
    {
        private Imap4Client _client;

        public ImapMailReader() { }

        private ImapMailReader(IIMapServerSettings mailConfig)
        {
            if (mailConfig.IsSecureSocket)
            {
                Client.ConnectSsl(mailConfig.ImapServer, mailConfig.Port);
            }
            else
            {
                Client.Connect(mailConfig.ImapServer, mailConfig.Port);
            }

            Client.Login(mailConfig.UserLogin, mailConfig.UserPassword);
        }

        public ImapMailReader CreateConnection(IIMapServerSettings mailConfig)
        {
            return new ImapMailReader(mailConfig);
        }

        public bool IsConnected()
        {
            return Client.IsConnected;
        }

        public bool HasUnreadMail()
        {
            try
            {
                Mailbox mails = Client.SelectMailbox("INBOX");
                int[] result = mails.Search("UNSEEN");
                return result.Length > 0;
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        public IReadOnlyCollection<Message> GetUnreadMails(string mailBox, int messageCount)
        {
            try
            {
                var mails = GetMails(mailBox, "UNSEEN", messageCount).Cast<Message>().ToArray();
                return mails;
            }
            catch (Exception)
            {
                // ignored
            }

            return new List<Message>();
        }

        protected Imap4Client Client
        {
            get { return _client ?? (_client = new Imap4Client()); }
        }

        private MessageCollection GetMails(string mailBox, string searchPhrase, int messageCount)
        {
            Mailbox inbox = Client.SelectMailbox(mailBox);
            MessageCollection messages = new MessageCollection();

            var unseenEmailIds = inbox.Search(searchPhrase).Take(messageCount);
            foreach (var messageId in unseenEmailIds)
            {
                var message = inbox.Fetch.MessageObject(messageId);
                messages.Add(message);
            }

            return messages;
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Disconnect();
                _client.Close();
            }
        }
    }
}