using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Library.Persistence
{
    internal class AbstractDataRepository<T, TC> : EntityValidationBase
        where T : class
        where TC : DbContext, new()
    {
        public virtual T Save(T entity)
        {
            try
            {
                using (var dbContext = new TC())
                {
                    dbContext.Entry(entity).State = EntityState.Added;
                    dbContext.SaveChanges();

                    return entity;
                }
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new StringBuilder();

                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.AppendLine(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                        DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(outputLines.ToString());  
            }
        }

        public virtual void Update(T entity)
        {
            ExecuteValidationExceptionHandledOperation(() =>
            {
                using (TC dbContext = new TC())
                {
                    dbContext.Entry(entity).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
            });
        }

        public virtual void DetachAndUpdate(int id, T updatedEntity)
        {
            ExecuteValidationExceptionHandledOperation(() =>
            {
                using (TC dbContext = new TC())
                {
                    T oldEntity = dbContext.Set<T>().Find(id);
                    dbContext.Entry(oldEntity).CurrentValues.SetValues(updatedEntity);
                    dbContext.SaveChanges();
                }
            });
        }

        public virtual void Delete(T entity)
        {
            using (TC dbContext = new TC())
            {
                dbContext.Entry(entity).State = EntityState.Deleted;
                dbContext.SaveChanges();
            }
        }


        public T Get(int id)
        {
            using (TC dbContext = new TC())
            {
                return dbContext.Set<T>().Find(id);
            }
        }

        /*********************** Result Paging *******************************/

        public int GetDataCount()
        {
            using (TC dbContext = new TC())
            {
                return dbContext.Set<T>().Count();
            }
        }

        public int GetDataCount(Expression<Func<T, bool>> predicate)
        {
            using (TC dbContext = new TC())
            {
                return dbContext.Set<T>().Count(predicate);
            }
        }

        public List<T> GetAll()
        {
            using (TC dbContext = new TC())
            {
                return dbContext.Set<T>().ToList();
            }
        }
    
        public IEnumerable<T> DbFilter(Expression<Func<T, bool>> filter)
        {
            using (TC dbContext = new TC())
            {
                return dbContext.Set<T>().Where(filter).ToArray();
            }
        }

        public void SaveAll(IEnumerable<T> list)
        {
            ExecuteValidationExceptionHandledOperation(() =>
            {
                using (TC dbContext = new TC())
                {
                    ExecuteValidationAgnosticOperation(dbContext, () =>
                    {
                        foreach (var item in list)
                        {
                            dbContext.Entry(item).State = EntityState.Added;
                        }

                        dbContext.SaveChanges();
                    });
                }
            });

        }

        public T UpdateAll(Expression<Func<T, bool>> queryExpression, T update)
        {
            T firstModifiedItem = null;

            ExecuteValidationExceptionHandledOperation(() =>
            {
                using (TC context = new TC())
                {
                    ExecuteValidationAgnosticOperation(context, () =>
                    {
                        var items = context.Set<T>().Where(queryExpression);

                        foreach (var item in items)
                        {
                            update.CopyTo(item);
                            context.Entry(item).State = EntityState.Modified;

                            if (firstModifiedItem == null) firstModifiedItem = item;
                        }

                        context.SaveChanges();
                    });
                }
            });

            return firstModifiedItem;
        }
    }
}
