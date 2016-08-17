using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Core.Persistence.Repositories
{
    public interface IInboundEmailRepository : IDataRepository<InboundEmail>
    {
        IList<InboundEmail> GetUnprocessedInboundEmails(int batchCount = 0);
        bool IsEmailProcessed(int emailId);
    }
}