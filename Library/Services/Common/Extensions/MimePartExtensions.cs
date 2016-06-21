using System.Linq;
using ActiveUp.Net.Mail;

namespace Library.Services.Common.Extensions
{
    public static class MimePartExtensions
    {
        private static readonly string[] ValidExtensions = {"DOC", "DOCX", "PDF"};

        public static bool HasValidExtension(this MimePart attachment)
        {
            var fileExtension = attachment.Filename.Split('.').LastOrDefault() ?? "INVALID";
            return ValidExtensions.Contains(fileExtension.ToUpper());
        }
    }
}
