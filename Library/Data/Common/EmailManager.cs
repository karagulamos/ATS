using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Library.Core.Bootstrapper;
using Library.Models;
using EmailFormatException = Library.Data.Common.Exceptions.EmailFormatException;

namespace Library.Data.Common
{
    [Export(typeof(IEmailManager))]
    [PartCreationPolicy(CreationPolicy.Any)]
    internal class EmailManager : IEmailManager
    {
        [Import]
        private IDataRepositoryFactory _dataRepositoryFactory;

        public EmailManager()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        public void Send(string from, string to, string subject, string body, bool isHtmlBody)
        {
            Send(from, to, subject, body, isHtmlBody, "");
        }

        public void Send(string from, string to, string subject, string body, bool isHtmlBody, string fileAttachment)
        {
            Send(from, new List<string> { to }, subject, body, isHtmlBody, String.IsNullOrEmpty(fileAttachment) ? new List<string>() : new List<string> { fileAttachment });
        }

        public void Send(string from, List<string> to, string subject, string body, bool isHtmlBody, List<string> fileAttachments)
        {
            var email = new Email
            {
                From = from,
                Body = body,
                IsHtml = isHtmlBody,
                Subject = subject,
                Recipients = new List<Recipient>(),
                Attachments = new List<Attachment>()
            };

            foreach (var address in to.Where(address => !String.IsNullOrEmpty(address)))
            {
                email.Recipients.Add(new Recipient
                {
                    EmailAddress = address
                });
            }

            foreach (var attachment in fileAttachments.Where(attachment => !String.IsNullOrEmpty(attachment)))
            {
                email.Attachments.Add(new Attachment
                {
                    FilePath = attachment
                });
            }

            Send(email);
        }

        private void Send(Email email)
        {
            if (String.IsNullOrEmpty(email.From))
                throw new EmailFormatException("From field cannot be blank");

            if (email.Recipients.Count <= 0)
                throw new EmailFormatException("A minimum of one recipient is required");

            if (String.IsNullOrEmpty(email.Body))
                throw new EmailFormatException("The body of the mail cannot be blank");

            foreach (var attachment in email.Attachments)
            {
                if (!File.Exists(attachment.FilePath))
                    throw new EmailFormatException("The attached file at " + attachment.FilePath + " does not exist");
            }

            _dataRepositoryFactory.Create<IOutboundEmailRepository>().Save((OutboundEmail)email);
        }
    }
}
