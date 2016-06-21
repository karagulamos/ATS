using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Library.Core.Bootstrapper;
using Library.Services.Helper;
using Library.Services.Logger;

namespace Library.Services.DocumentExtractors
{
    class PdfDocumentExtractor : IDocumentExtractor
    {
        [Import]
        private ILoggerService _logger;

        [Import] 
        private IPatternMatcher _patternMatcher;

        public PdfDocumentExtractor()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
        public List<string> GetRows(string documentPath, ICollection<string> stopWords = null, string[] skipWords = null)
        {
            try
            {
                _logger.Info("=== ENTERING PDF DOCUMENT EXTRACTOR ===");
                _logger.Debug("Retrieving document stored at : " + documentPath);

                using (PdfReader reader = new PdfReader(documentPath))
                {
                    _logger.Info(documentPath + " successfully retrieved.");

                    _logger.Debug("Preparing to read and process PDF content of " + documentPath);
                    ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                    List<string> parsedLines = new List<string>();

                    _logger.Info("PDF stream successfully read: " + documentPath);

                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string page = PdfTextExtractor.GetTextFromPage(reader, i, strategy);

                        if (skipWords != null && skipWords.Any(s => page.Contains(s)))
                            continue;

                        parsedLines.AddRange(page.Split('\n'));
                    }

                    if (_patternMatcher == null)
                        _patternMatcher = new NullPatternMatcher();

                    if (stopWords != null)
                        parsedLines = parsedLines.TakeWhile(line => !stopWords.Any(line.Contains))
                                                 .Union(_patternMatcher.GetMatchedRows(parsedLines))
                                                 .ToList();

                    _logger.Info(documentPath + " PDF stream successfully processed");
                    _logger.Info(parsedLines.Count + " rows processed and retrieved.");

                    return parsedLines;
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Error("ArgumentOutOfRangeException occurred: " + ex);
            }
            catch (Exception exception)
            {
                _logger.Error("Unknown exception occurred: " + exception);
            }

            return new List<string>();
        }
    }
}
