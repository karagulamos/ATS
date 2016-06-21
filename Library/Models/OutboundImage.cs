using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public partial class OutboundImage
    {
        [Key]
        public int OutboundImageId { get; set; }
        [ForeignKey("OutboundEmail")]
        public int OutboundEmailId { get; set; }
        public string FilePath { get; set; }
    
        public virtual OutboundEmail OutboundEmail { get; set; }
    }
}
