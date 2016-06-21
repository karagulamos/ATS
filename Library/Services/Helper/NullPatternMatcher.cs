using System.Collections.Generic;

namespace Library.Services.Helper
{
    public class NullPatternMatcher : IPatternMatcher
    {
        public IEnumerable<string> GetMatchedRows(List<string> itemsToScan)
        {
            return new List<string>();
        }
    }
}