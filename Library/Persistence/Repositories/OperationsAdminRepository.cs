using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Library.Core.Models;
using Library.Core.Persistence.Repositories;

namespace Library.Persistence.Repositories
{
    [Export(typeof(IOperationsAdminRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class OperationsAdminRepository : AbstractDataRepository<OperationsAdmin, AtsDbContext>, IOperationsAdminRepository
    {
        public List<string> GetOperationsAdminEmails()
        {
            using (var context = new AtsDbContext())
            {
                var emails = 
                    from admin in context.OperationsAdmins
                    select admin.Email;

                return emails.ToList();
            }
        }
    }
}
