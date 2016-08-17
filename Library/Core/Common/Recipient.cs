using Library.Core.Models;

namespace Library.Core.Common
{
    public class Recipient
    {
        public string EmailAddress { get; set; }

        public static explicit operator OutboundRecipient(Recipient recipient)
        {
            return new OutboundRecipient
            {
                RecipientEmail = recipient.EmailAddress
            };
        }
    }
}
