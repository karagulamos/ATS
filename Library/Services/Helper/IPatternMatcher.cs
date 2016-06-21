using System.Collections.Generic;

namespace Library.Services.Helper
{
    public interface IPatternMatcher
    {
        IEnumerable<string> GetMatchedRows(List<string> itemsToScan);
    }
}