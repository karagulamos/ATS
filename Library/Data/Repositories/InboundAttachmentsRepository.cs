using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Library.Models;

namespace Library.Data.Repositories
{
    [Export(typeof(IInboundAttachmentsRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class InboundAttachmentsRepository : AbstractDataManager<InboundAttachment, AtsDbContext>, IInboundAttachmentsRepository
    {

        public InboundAttachment[] GetUnprocessedAttachments(int batchCount)
        {
            using (var dbContext = new AtsDbContext())
            {
                var attachs = from attachment in dbContext.InboundAttachments
                              join email in dbContext.InboundEmails on attachment.InboundEmailId equals email.InboundEmailId
                              where email.Processed == 0
                              select attachment;

                if (batchCount > 0)
                    return attachs.Take(batchCount).Include(a => a.InboundEmail).ToArray();

                return attachs.Include(a => a.InboundEmail).ToArray();
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
