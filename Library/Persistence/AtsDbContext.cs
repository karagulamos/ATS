using System.Data.Entity;
using Library.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Library.Persistence
{
    public class AtsDbContext : IdentityDbContext<AppUser>
    {
        public AtsDbContext() : base("ATS")
        {
            Database.SetInitializer(new NullDatabaseInitializer<AtsDbContext>());
        }

        public DbSet<SmtpDetail> SmtpDetails { get; set; }
        public DbSet<InboundEmail> InboundEmails { get; set; }
        public DbSet<InboundAttachment> InboundAttachments { get; set; }
        public DbSet<OutboundEmail> OutboundEmails { get; set; }
        public DbSet<OutboundAttachment> OutboundAttachments { get; set; }
        public DbSet<OutboundImage> OutboundImages { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<OperationsAdmin> OperationsAdmins { get; set; }
    }
}
