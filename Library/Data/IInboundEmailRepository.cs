using System.Collections.Generic;
using Library.Data.Managers;
using Library.Models;

namespace Library.Data
{
    public interface IInboundEmailRepository : IDataManager<InboundEmail>
    {
        IList<InboundEmail> GetUnprocessedInboundEmails(int batchCount = 0);
        bool IsEmailProcessed(int emailId);
    }
}