using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Library.Data.Managers;
using Library.Models;

namespace Library.Data
{
    public interface ISmtpDetailRepository : IDataManager<SmtpDetail>
    {
        IEnumerable<T> GetImapSettings<T>(Expression<Func<SmtpDetail, T>> action);
    }
}