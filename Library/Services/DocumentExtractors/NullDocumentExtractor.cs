using System.Collections.Generic;
using Library.Services.Helper;

namespace Library.Services.DocumentExtractors
{
    class NullDocumentExtractor : IDocumentExtractor
    {
        public List<string> GetRows(string documentPath, ICollection<string> stopWords = null, string[] skipWords = null, IPatternMatcher patternMatcher = null)
        {
            return new List<string>();
        }
    }
}
