﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using Library.Core.Models;
using Library.Core.Persistence.Repositories;

namespace Library.Persistence.Repositories
{
    [Export(typeof(IInboundEmailRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class InboundEmailRepository : AbstractDataRepository<InboundEmail, AtsDbContext>, IInboundEmailRepository
    {
        public IList<InboundEmail> GetUnprocessedInboundEmails(int batchCount)
        {
            using (var dbContext = CreateDataContext())
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
            using (var dbContext = CreateDataContext())
            {
                return dbContext.InboundEmails.Any(e => e.InboundEmailId == emailId && e.Processed == 1);
            }
        }
    }
}
