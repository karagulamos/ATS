using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using Library.Core.Bootstrapper;
using Library.Core.Messaging.Infrastructure;
using Library.Core.Messaging.Settings;
using Library.Core.Models;
using Library.Core.Persistence.Repositories;
using Library.Services.Common.Extensions;

namespace Library.Services.Tasks
{
    [Export(TaskType.EmailListener, typeof(ITaskRunner))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class EmailListener : ITaskRunner
    {
        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        IImapSessionManager _imapSessionManager;

        private readonly int _emailMessageCount;

        public EmailListener()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);

            _emailMessageCount = Convert.ToInt32(ConfigurationManager.AppSettings["UnprocessedEmailBatchCount"]);
        }

        // For Unit / Integration Testing
        public EmailListener(IDataRepositoryFactory dataRepositoryFactory, IImapSessionManager imapSessionManager)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _imapSessionManager = imapSessionManager;
        }

        public async void Execute()
        {
            var smtpSettings = LoadSmtpSettings().ToArray();

            if (smtpSettings.Length > 0)
            {
                _imapSessionManager.BuildSessionCache(smtpSettings);

                if (!_imapSessionManager.IsSessionCacheEmpty)
                {
                    this.FetchAndSaveUnreadEmails();
                }
            }
        }

        private IEnumerable<IIMapServerSettings> LoadSmtpSettings()
        {
            var smtpRepostory = _dataRepositoryFactory.Create<ISmtpDetailRepository>();

            var settings = smtpRepostory.GetImapSettings(m => new ImapServerSettings
            {
                ImapServer = m.Host,
                IsSecureSocket = m.UseSSL == 1,
                Port = m.Port,
                UserLogin = m.Username,
                UserPassword = m.Password,
                RequestingEntityId = m.SmtpDetailId
            });

            return settings;
        }

        private void FetchAndSaveUnreadEmails()
        {
            var inboxMails = _imapSessionManager.GetUnreadInboxMails();

            if (inboxMails.Count > 0)
            {
                var unreadEmails = from mail in inboxMails
                                   let unreadMail = mail.Message
                                   let setting = mail.ImapSetting
                                   select new InboundEmail
                                   {
                                       Subject = unreadMail.Subject ?? "",
                                       Sender = unreadMail.From.Email,
                                       SenderName = unreadMail.From.Name,
                                       SenderIP = unreadMail.SenderIP != null ? unreadMail.SenderIP.ToString() : "N/A",
                                       BodyText = unreadMail.BodyHtml.Text,
                                       Recipient = setting.UserLogin,
                                       SmtpDetailId = setting.RequestingEntityId,
                                       DateReceived = unreadMail.ReceivedDate,
                                       DateCreated = DateTime.UtcNow,
                                       Processed = 0
                                   }.IncludeAttachmentsOrDefault(unreadMail.Attachments);

                _dataRepositoryFactory.Create<IInboundEmailRepository>().SaveAll(unreadEmails);
            }
        }
    }
}
