using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public partial class OutboundEmail
    {
        public OutboundEmail()
        {
            OutboundAttachments = new HashSet<OutboundAttachment>();
            OutboundRecipients = new HashSet<OutboundRecipient>();
            OutboundImages = new HashSet<OutboundImage>();
        }
    
        [Key]
        public int OutboundEmailId { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime DateCreated { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int EmailStatusId { get; set; }
        public int? ErrorId { get; set; }
        public int Attempts { get; set; }
        public bool IsHtml { get; set; }
    
        public ICollection<OutboundAttachment> OutboundAttachments { get; set; }
        public ICollection<OutboundRecipient> OutboundRecipients { get; set; }
        public ICollection<OutboundImage> OutboundImages { get; set; }
    }
}
