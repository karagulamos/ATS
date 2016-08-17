using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Core.Persistence.Repositories
{
    public interface IOperationsAdminRepository : IDataRepository<OperationsAdmin>
    {
        List<string> GetOperationsAdminEmails();
    }
}