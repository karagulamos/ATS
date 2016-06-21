using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public partial class InboundAttachment
    {
        [Key]
        public int InboundAttachmentId { get; set; }

        [ForeignKey("InboundEmail")]
        public int InboundEmailId { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }
        public string FileType { get; set; }
    
        public InboundEmail InboundEmail { get; set; }
    }
}
