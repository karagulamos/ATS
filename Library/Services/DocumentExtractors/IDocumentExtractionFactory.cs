namespace Library.Services.DocumentExtractors
{
    public interface IDocumentExtractorFactory
    {
        IDocumentExtractor GetExtractor(string documentType);
    }
}