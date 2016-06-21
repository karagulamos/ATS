using System.Collections.Generic;
using System.Linq;

namespace Library.Services.Common.Extensions
{
    public static class CustomExtensions
    {
        public static bool IsValidCvDocument(this IEnumerable<string> parsedRows)
        {
            return !parsedRows.All(
                    row => row.Contains("Dear") 
                        || row.Contains("Sir")
                        || row.Contains("Madam")
                        || row.Contains("Re:")
                        || row.Contains("APPLICATION")
                        || row.Contains("hereby")
                        || row.Contains("wish to apply")
                        || row.Contains("Yours")
                        || row.ToLower().Contains("faithfully")
                        || row.ToLower().Contains("sincerely")
                );
        }
    }
}
