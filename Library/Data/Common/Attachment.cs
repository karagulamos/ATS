using Library.Models;

namespace Library.Data.Common
{
    public class Attachment
    {
        public string FilePath { get; set; }

        public static explicit operator OutboundAttachment(Attachment attachment)
        {
            return new OutboundAttachment
            {
                FilePath = attachment.FilePath
            };
        }
    }
}
