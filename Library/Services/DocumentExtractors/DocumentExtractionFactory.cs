using System.ComponentModel.Composition;

namespace Library.Services.DocumentExtractors
{
    [Export(typeof(IDocumentExtractorFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class DocumentExtractorFactory : IDocumentExtractorFactory
    {
        private readonly IDocumentExtractor _pdfDocumentExtractor = new PdfDocumentExtractor();
        private readonly IDocumentExtractor _wordDocumentExtractor = new WordDocumentExtractor();

        public IDocumentExtractor GetExtractor(string documentType)
        {
            switch (documentType)
            {
                case "DOC":
                case "DOCX":
                    return _wordDocumentExtractor;

                case "PDF":
                    return _pdfDocumentExtractor;
            }

            return new NullDocumentExtractor();
        }
    }
}