using System.Collections.Generic;
using Library.Data.Managers;
using Library.Models;

namespace Library.Data
{
    public interface IOperationsAdminRepository : IDataManager<OperationsAdmin>
    {
        List<string> GetOperationsAdminEmails();
    }
}