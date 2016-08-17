using System;
using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Core.Common
{
    public class Email
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public int ErrorId { get; set; }
        public List<Recipient> Recipients { get; set; }
        public List<Attachment> Attachments { get; set; }

        public static explicit operator OutboundEmail(Email email)
        {
            var dbemail = new OutboundEmail
            {
                Sender = email.From,
                Body = email.Body,
                IsHtml = email.IsHtml,
                Subject = email.Subject,
                DateCreated = DateTime.Now,
                DateSent = DateTime.Now,
                EmailStatusId = (int) EmailStatuses.New,
                Attempts = 0,
                ErrorId = null,
                OutboundAttachments = new List<OutboundAttachment>(),
                OutboundRecipients = new List<OutboundRecipient>()
            };

            email.Attachments.ForEach(att => dbemail.OutboundAttachments.Add((OutboundAttachment)att));
            email.Recipients.ForEach(rec=> dbemail.OutboundRecipients.Add((OutboundRecipient)rec));
            return dbemail;
        }
    }
}
