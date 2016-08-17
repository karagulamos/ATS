using System.Data.Entity;
using Library.Core.Models;
using Library.Persistence;

namespace Library.Core.Persistence
{
    public interface IAtsDbContext // for mocking and unit testing
    {
        DbSet<SmtpDetail> SmtpDetails { get; set; }
        DbSet<InboundEmail> InboundEmails { get; set; }
        DbSet<InboundAttachment> InboundAttachments { get; set; }
        DbSet<OutboundEmail> OutboundEmails { get; set; }
        DbSet<OutboundAttachment> OutboundAttachments { get; set; }
        DbSet<OutboundImage> OutboundImages { get; set; }
        DbSet<Candidate> Candidates { get; set; }
        DbSet<OperationsAdmin> OperationsAdmins { get; set; }
        IDbSet<AppUser> Users { get; set; }
    }
}