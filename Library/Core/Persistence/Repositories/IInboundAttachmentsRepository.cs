using Library.Core.Models;

namespace Library.Core.Persistence.Repositories
{
    public interface IInboundAttachmentsRepository : IDataRepository<InboundAttachment>
    {
        InboundAttachment[] GetUnprocessedAttachments(int batchCount);
        bool VerifyInboundEmailHasAttachment(int? email);
    }
}