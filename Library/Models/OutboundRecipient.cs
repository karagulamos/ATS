using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public partial class OutboundRecipient
    {
        [Key]
        public int OutboundRecipientId { get; set; }
        [ForeignKey("OutboundEmail")]
        public int OutboundEmailId { get; set; }
        public string RecipientEmail { get; set; }

        public virtual OutboundEmail OutboundEmail { get; set; }
    }
}
