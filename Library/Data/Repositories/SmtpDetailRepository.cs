using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using Library.Models;

namespace Library.Data.Repositories
{
    [Export(typeof(ISmtpDetailRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class SmtpDetailRepository : AbstractDataManager<SmtpDetail, AtsDbContext>, ISmtpDetailRepository
    {
        public IEnumerable<T> GetImapSettings<T>(Expression<Func<SmtpDetail, T>> action)
        {
            using (var dbContext = new AtsDbContext())
            {
                return dbContext.SmtpDetails.Select(action).ToArray();
            }
        }
    }
}
