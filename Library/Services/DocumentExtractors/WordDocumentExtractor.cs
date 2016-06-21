using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Library.Core.Bootstrapper;
using Library.Data;
using Library.Services.Helper;
using Library.Services.Logger;
using Spire.Doc;

namespace Library.Services.DocumentExtractors
{
    class WordDocumentExtractor : IDocumentExtractor
    {
        [Import]
        private ILoggerService _logger;
        public WordDocumentExtractor()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
        public List<string> GetRows(string documentPath, ICollection<string> stopWords = null, string[] skipWords = null, IPatternMatcher patternMatcher = null)
        {
            var memoryStream = new MemoryStream();

            try
            {
                _logger.Info("=== ENTERING WORD DOCUMENT EXTRACTOR ===");

                _logger.Debug("Retrieving document stored at : " + documentPath);
                Document document = new Document(documentPath);
                _logger.Info(documentPath + " successfully retrieved.");

                _logger.Debug("Converting and saving document " + documentPath + " as PDF in memory.");

                ThrowIfTimedOut(
                    () => document.SaveToFile(memoryStream, FileFormat.PDF),
                    TimeSpan.FromSeconds(10)
                );

                _logger.Info(documentPath + " successfully converted to PDF.");
                memoryStream.Position = 0;

                using (PdfReader reader = new PdfReader(memoryStream))
                {
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

                    if (patternMatcher == null)
                        patternMatcher = new NullPatternMatcher();

                    if (stopWords != null)
                        parsedLines = parsedLines.TakeWhile(line => !stopWords.Any(line.Contains))
                            .Union(patternMatcher.GetMatchedRows(parsedLines))
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
            finally
            {
                memoryStream.Dispose();
            }

            return new List<string>();
        }

        private static void ThrowIfTimedOut(Action action, TimeSpan timeout)
        {
            if (!Task.Run(action).Wait(timeout))
               throw new TimeoutException("Operation Timed Out.");
        }
    }
}
