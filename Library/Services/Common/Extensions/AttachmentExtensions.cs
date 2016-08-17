using System;
using System.Collections.Generic;
using System.Linq;
using Library.Core.Models;

namespace Library.Services.Common.Extensions
{
    public static class AttachmentExtensions
    {
        public static bool IsWordDocument(this InboundAttachment attachment)
        {
            switch (attachment.FileType)
            {
                case "DOC":
                case "DOCX":
                    return true;
            }

            return false;
        }

        public static bool IsPdfDocument(this InboundAttachment attachment)
        {
            return attachment.FileType == "PDF";
        }

        public static IEnumerable<InboundAttachment> TryGetValidCvsOrDefault(this IEnumerable<InboundAttachment> attachments, string senderName)
        {
            var names = senderName.ToLower().Split(new []{' ', '-'}, StringSplitOptions.RemoveEmptyEntries);
            var allAttachments = attachments.ToList();

            var validatedCvs = allAttachments.Where(a =>
            {
                var file = a.OriginalFileName.ToLower();

                return
                    file.Contains("cv") || file.Contains("resume")
                 || file.Contains("vitae") || file.Contains("curricu")
                 || names.Any(name => file.Contains(name.Trim()));
            }).ToList();

            return validatedCvs.Count > 0 ? validatedCvs : allAttachments;
        }

        public static bool IsValidCvFile(this InboundAttachment attachment)
        {
            var names = attachment.InboundEmail.SenderName.ToLower().Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            var file = attachment.OriginalFileName.ToLower();

            return  file.Contains("cv") || file.Contains("resume") 
                 || file.Contains("vitae") || file.Contains("curricu")
                 || names.Any(name => file.Contains(name.Trim()));
        }
    }
}
