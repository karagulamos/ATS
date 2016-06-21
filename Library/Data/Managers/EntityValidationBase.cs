using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Library.Data.Managers
{
    internal class EntityValidationBase
    {
        protected static void ExecuteValidationExceptionHandledOperation(Action dbcontextEntityOperation)
        {
            try
            {

                dbcontextEntityOperation.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                                      .SelectMany(x => x.ValidationErrors)
                                      .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }


        protected static void ExecuteValidationAgnosticOperation<TU>(TU dbContext, Action dbcontextEntityOperation) where TU : DbContext
        {
            try
            {
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                dbContext.Configuration.ValidateOnSaveEnabled = false;

                dbcontextEntityOperation.Invoke();
            }
            finally
            {
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                dbContext.Configuration.ValidateOnSaveEnabled = true;
            }
        }
    }
}