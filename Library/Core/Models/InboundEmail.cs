using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public partial class InboundEmail
    {
        public InboundEmail()
        {
            InboundAttachments = new HashSet<InboundAttachment>();
        }
    
        [Key]
        public int InboundEmailId { get; set; }
        public System.DateTime DateReceived { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string Recipient { get; set; }
        public string SenderIP { get; set; }
        [ForeignKey("SmtpDetail")]
        public int SmtpDetailId { get; set; }
        public int Processed { get; set; }
        public string BodyText { get; set; }
    
        public SmtpDetail SmtpDetail { get; set; }
        public ICollection<InboundAttachment> InboundAttachments { get; set; }
    }
}
