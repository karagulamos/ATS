using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Library.Core.Models;

namespace Library.Core.Persistence.Repositories
{
    public interface ISmtpDetailRepository : IDataRepository<SmtpDetail>
    {
        IEnumerable<T> GetImapSettings<T>(Expression<Func<SmtpDetail, T>> action);
    }
}