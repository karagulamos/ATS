using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Library.Models;

namespace Library.Data.Repositories
{
    [Export(typeof(IInboundEmailRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class InboundEmailRepository : AbstractDataManager<InboundEmail, AtsDbContext>, IInboundEmailRepository
    {
        public IList<InboundEmail> GetUnprocessedInboundEmails(int batchCount)
        {
            using (var dbContext = new AtsDbContext())
            {
                var emails = from email in dbContext.InboundEmails 
                             where email.Processed == 0
                             select email;

                if (batchCount > 0)
                    emails = emails.Take(batchCount);

                return emails.Include(e => e.InboundAttachments).ToList();
            }
        }

        public bool IsEmailProcessed(int emailId)
        {
            using (var dbContext = new AtsDbContext())
            {
                return dbContext.InboundEmails.Any(e => e.InboundEmailId == emailId && e.Processed == 1);
            }
        }
    }
}
