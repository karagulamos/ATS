using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ActiveUp.Net.Mail;
using Library.Core.Models;

namespace Library.Services.Common.Extensions
{
    public static class InboundEmailExtensions
    {
        public static InboundEmail IncludeAttachmentsOrDefault(this InboundEmail email, AttachmentCollection incomingAttachments)
        {
            if (incomingAttachments.Count <= 0) return email;

            var filePath = ConfigurationManager.AppSettings["InboundAttachmentsPath"];

            var attachments = incomingAttachments.GetEnumerator();

            while (attachments.MoveNext())
            {
                var attachment = attachments.Current as MimePart;

                if (attachment != null && attachment.HasValidExtension()) // Skip invalid attachments
                {
                    var fileExtension = attachment.Filename.Split('.').LastOrDefault() ?? "INVALID";

                    if (filePath.Last() != '\\') filePath = filePath + @"\";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    var completeUniqueFilePath = GenerateUniqueFilePath(filePath, fileExtension);
                    
                    attachment.StoreToFile(completeUniqueFilePath);

                    email.InboundAttachments.Add(new InboundAttachment
                    {
                        FilePath = completeUniqueFilePath,
                        OriginalFileName = attachment.Filename,
                        FileType = fileExtension.ToUpper()
                    });
                }
            }

            return email;
        }

        private static long _lastTimeStamp = DateTime.UtcNow.Ticks;
        private static string GenerateUniqueFilePath(string filePath, string fileExtension)
        {
            long originalTimeStamp, newTimeStamp;
            do
            {
                originalTimeStamp = _lastTimeStamp;
                var now = DateTime.UtcNow.Ticks;
                newTimeStamp = Math.Max(now, originalTimeStamp + 1);
            } while (Interlocked.CompareExchange (ref _lastTimeStamp, newTimeStamp, originalTimeStamp) != originalTimeStamp);

            var prependedToStamp = Regex.Replace(DateTime.UtcNow.ToString("O"), @"[^0-9T]", string.Empty);
            return string.Format("{0}{1}{2}.{3}", filePath, prependedToStamp, newTimeStamp, fileExtension);
        }
    }
}
