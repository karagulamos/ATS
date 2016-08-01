using System.Collections.Generic;

namespace Library.Services.DocumentExtractors
{
    public interface IDocumentExtractor
    {
        List<string> GetRows(string documentPath, ICollection<string> stopWords = null, string[] skipWords = null);
    }
}