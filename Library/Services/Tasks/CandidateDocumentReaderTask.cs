using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using Library.Data;
using Library.Data.Common;
using Library.Models;
using Library.Services.Common.Extensions;
using Library.Services.DocumentExtractors;
using Library.Services.Helper;
using Library.Services.Validation;

namespace Library.Services.Tasks
{
    [Export(TaskType.CandidateDocumentReader, typeof(ITaskRunner))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class CandidateDocumentReaderTask : ITaskRunner
    {
        [Import]
        IDataRepositoryFactory _dataRepositoryFactory;

        [Import]
        private ICandidateBuilder _candidateBuilder;

        [Import]
        private IDocumentExtractorFactory _documentExtractorFactory;

        [Import]
        private IEmailManager _emailManager;

        [Import]
        private IRegexCompiler _regexCompiler;

        [Import]
        private IPatternMatcher _patternMatcher;

        public void Execute()
        {
            int batchCount = Convert.ToInt32(ConfigurationManager.AppSettings["UnprocessedEmailBatchCount"]);
            var emails = _dataRepositoryFactory.Create<IInboundEmailRepository>()
                                               .GetUnprocessedInboundEmails(batchCount);

            foreach (var email in emails)
            {
                foreach (var attachment in email.InboundAttachments.TryGetValidCvsOrDefault(email.SenderName))
                {
                    var documentExtractor = _documentExtractorFactory.GetExtractor(attachment.FileType);
                    var parsedRows = documentExtractor.GetRows(attachment.FilePath, ResumeFilterHelper.GetStopWords(), ResumeFilterHelper.GetSkipWords(), _patternMatcher);

                    if (parsedRows.Count <= 0) continue;

                    var candidate = _candidateBuilder.BuildFrom(parsedRows, _regexCompiler);

                    if (candidate.IsValidCandidate())
                    {
                        ValidateSenderDetails(candidate, parsedRows, email.SenderName, email.Sender);

                        candidate.InboundEmailId = email.InboundEmailId;
                        candidate.InboundAttachmentId = attachment.InboundAttachmentId;

                        _dataRepositoryFactory.Create<ICandidateRepository>().Save(candidate);

                        var incompleteCandidateDetails = candidate.GetIncompleteCandidateDetails();

                        if (incompleteCandidateDetails.Count > 0)
                            SendIncompleteDetailsNotification(candidate, incompleteCandidateDetails);
                    }
                }

                email.Processed = 1;
                _dataRepositoryFactory.Create<IInboundEmailRepository>()
                                      .DetachAndUpdate(email.InboundEmailId, email);
            }
        }

        private static void ValidateSenderDetails(Candidate candidate, IEnumerable<string> dataRows, string senderName, string senderEmail)
        {
            if (string.IsNullOrEmpty(candidate.Email))
                candidate.Email = senderEmail;

            var senderNames = senderName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var dataRow in dataRows)
            {
                var row = dataRow.ToLower();
                if (senderNames.Any(name => row.Contains(name.ToLower())))
                {
                    candidate.FirstName = string.IsNullOrEmpty(senderNames[0]) ? candidate.FirstName : senderNames[0].ToTitleCase();
                    candidate.LastName = senderNames.Count > 1 && !string.IsNullOrEmpty(senderNames[1]) ? senderNames[1].ToTitleCase() : candidate.LastName;
                    break;
                }
            }
        }

        private void SendIncompleteDetailsNotification(Candidate candidate, IEnumerable<string> unavailableDetails)
        {
            var messageBody = string.Format(ResumeFilterHelper.GetNotificationTemplate(),
                candidate.FirstName ?? "X",
                candidate.LastName ?? "X",
                candidate.Age > 0 ? candidate.Age.ToString() : "X",
                candidate.Email ?? "X",
                candidate.PhoneNumber ?? "X",
                candidate.StateOfOrigin ?? "X",
                unavailableDetails.Aggregate(string.Empty, (current, row) => current + (!string.IsNullOrEmpty(current) ? ", " : "") + row)
            );

            var admins = _dataRepositoryFactory.Create<IOperationsAdminRepository>().GetOperationsAdminEmails();

            if (admins.Count > 0)
                _emailManager.Send("ATS Service", admins, "ATS Applicant Notification", messageBody, true, new List<string>());
        }
    }
}
