using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Mail;
using Library.Core.Bootstrapper;
using Library.Core.Common;
using Library.Core.Persistence.Repositories;
using Library.Services.Tasks.Config;
using Attachment = System.Net.Mail.Attachment;

namespace Library.Services.Tasks
{
    [Export(TaskType.EmailSender, typeof(ITaskRunner))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class EmailSender : ITaskRunner
    {
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        private IOutboundEmailConfigurator _emailConfigurator;

        public EmailSender()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        // For Unit Test / Mocking
        public EmailSender(IDataRepositoryFactory dataRepositoryFactory, IOutboundEmailConfigurator emailConfigurator)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _emailConfigurator = emailConfigurator;
        }

        public void Execute()
        {
            var emails = _dataRepositoryFactory.Create<IOutboundEmailRepository>().GetOutboundEmailDetails();

            var emailCount = emails.Count;

            if (emailCount > 0)
            {
                foreach (var email in emails)
                {
                    var smptClient = _emailConfigurator.ConfigureSmtpClient(email);
                    var sendStatus = SendEmail(email, smptClient);
                    UpdateEmail(email, sendStatus);
                };
            }
        }

        private void UpdateEmail(OutboundEmailDetail email, bool sendSuccesful)
        {
            email.OutboundEmail.DateSent = DateTime.Now;
            email.OutboundEmail.Attempts++;
            email.OutboundEmail.EmailStatusId = sendSuccesful ? (int)EmailStatuses.Sent : (int)EmailStatuses.Failed;

            _dataRepositoryFactory.Create<IOutboundEmailRepository>().DetachAndUpdate(email.OutboundEmail.OutboundEmailId, email.OutboundEmail);
        }

        private bool SendEmail(OutboundEmailDetail email, SmtpClient smtpClient)
        {

            using (var mail = CreateMailMessage(email))
            {
                smtpClient.Send(mail);
            };

            return true;
        }

        private MailMessage CreateMailMessage(OutboundEmailDetail detail)
        {
            var email = detail.OutboundEmail;
            var message = new MailMessage
            {
                Subject = email.Subject,
                // Body = email.Body,
                IsBodyHtml = email.IsHtml
            };

            foreach (var recipient in email.OutboundRecipients)
            {
                message.To.Add(recipient.RecipientEmail);
            }

            foreach (var dbAttachment in email.OutboundAttachments)
            {
                var attachment = new Attachment(dbAttachment.FilePath);
                message.Attachments.Add(attachment);
            }

            var view = AlternateView.CreateAlternateViewFromString(email.Body, null, "text/html");
            if (email.OutboundImages != null && email.OutboundImages.Count > 0)
            {
                foreach (var image in email.OutboundImages)
                {
                    var inlineLogo = new LinkedResource(image.FilePath)
                    {
                        ContentId = Path.GetFileNameWithoutExtension(image.FilePath)
                    };
                    view.LinkedResources.Add(inlineLogo);
                }
            }

            message.AlternateViews.Add(view);
            var from = _emailConfigurator.ConfigureMailAddress(detail);
            message.From = from;
            return message;
        }
    }
}
