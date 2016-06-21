using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }

        [ForeignKey("InboundEmail")]
        public int InboundEmailId { get; set; }

        [ForeignKey("InboundAttachment")]
        public int InboundAttachmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string StateOfOrigin { get; set; }
        public string Email { get; set; }

        public int Age { get; set; }

        public InboundEmail InboundEmail { get; set; }
        public InboundAttachment InboundAttachment { get; set; }
    }
}
