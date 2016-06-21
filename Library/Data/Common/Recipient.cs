using Library.Models;

namespace Library.Data.Common
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
