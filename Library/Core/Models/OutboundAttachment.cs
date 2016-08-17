using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Core.Models
{
    public partial class OutboundAttachment
    {
        [Key]
        public int OutboundAttachmentId { get; set; }

        [ForeignKey("OutboundEmail")]
        public int OutboundEmailId { get; set; }
        public string FilePath { get; set; }
    
        public virtual OutboundEmail OutboundEmail { get; set; }
    }
}
