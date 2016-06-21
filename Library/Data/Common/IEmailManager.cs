using System.Collections.Generic;

namespace Library.Data.Common
{
    public interface IEmailManager
    {
        void Send(string from, string to, string subject, string body, bool isHtmlBody);
        void Send(string from, string to, string subject, string body, bool isHtmlBody, string fileAttachment );
        void Send(string from, List<string> to, string subject, string body, bool isHtmlBody, List<string> fileAttachments);
    }
}
