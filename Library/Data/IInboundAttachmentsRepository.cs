using Library.Data.Managers;
using Library.Models;

namespace Library.Data
{
    public interface IInboundAttachmentsRepository : IDataManager<InboundAttachment>
    {
        InboundAttachment[] GetUnprocessedAttachments(int batchCount);
        bool VerifyInboundEmailHasAttachment(int? email);
    }
}