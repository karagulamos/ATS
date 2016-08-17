using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Library.Core.Models;
using Library.Core.Persistence.Repositories;

namespace Library.Persistence.Repositories
{
    [Export(typeof(IInboundAttachmentsRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class InboundAttachmentsRepository : AbstractDataRepository<InboundAttachment, AtsDbContext>, IInboundAttachmentsRepository
    {

        public InboundAttachment[] GetUnprocessedAttachments(int batchCount)
        {
            using (var dbContext = new AtsDbContext())
            {
                var attachments = from attachment in dbContext.InboundAttachments
                              join email in dbContext.InboundEmails on attachment.InboundEmailId equals email.InboundEmailId
                              where email.Processed == 0
                              select attachment;

                if (batchCount > 0)
                    return attachments.Take(batchCount).Include(a => a.InboundEmail).ToArray();

                return attachments.Include(a => a.InboundEmail).ToArray();
            }
        }
        public bool VerifyInboundEmailHasAttachment(int? emailId)
        {
            using (var dbContext = new AtsDbContext())
            {
                return dbContext.InboundAttachments.Any(a => a.InboundEmailId == emailId);
            }
        }
    }
}
