using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using Library.Core.Bootstrapper;
using Library.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    class AgeValidator : IResumeValidator<Candidate>
    {
        public AgeValidator()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }

        public void Validate(Candidate candidate, List<string> dataRows, IRegexCompiler regexCompiler)
        {
            var dobRegex = regexCompiler.Compile(RegexOptionHelper.DobRegex);
            var ageRegex = regexCompiler.Compile(RegexOptionHelper.AgeRegex);
            var lastResortDobRegex = regexCompiler.Compile(RegexOptionHelper.LastResortDobRegex);

            foreach (var dataRow in dataRows)
            {
                // DOB REGEX
                if (dobRegex.IsMatch(dataRow.Trim()))
                {
                    var dobMatches = dobRegex.Match(dataRow.Trim()).Groups;
                    var year = !string.IsNullOrEmpty(dobMatches[4].Value)
                        ? dobMatches[4].Value
                        : dobMatches[3].Value;

                    candidate.Age = CalculateCandidateAge(Regex.Replace(year, @"[^0-9]", string.Empty));
                    break;
                }

                // AGE REGEX
                if (ageRegex.IsMatch(dataRow.Trim()))
                {
                    var ageMatches = ageRegex.Match(dataRow).Groups;
                    var possibleAge = Regex.Replace(ageMatches[1].Value, @"[^0-9]", string.Empty);
                    if (!string.IsNullOrEmpty(possibleAge) && possibleAge.Length == 2)
                    {
                        candidate.Age = Convert.ToInt32(possibleAge);
                        return;
                    }

                    var yearInTwoDigits = DateTime.Now.Year % 100;
                    candidate.Age = Convert.ToInt32(Convert.ToInt32(possibleAge) > yearInTwoDigits ? possibleAge : "20" + possibleAge);
                    break;
                }

                // LAST RESORT DOB REGEX
                if (lastResortDobRegex.IsMatch(dataRow.Trim()))
                {
                    var ageMatch = lastResortDobRegex.Match(dataRow);
                    candidate.Age = CalculateCandidateAge(Regex.Replace(ageMatch.Value, @"[^0-9]", string.Empty));
                    break;
                }
            }

        }

        private int CalculateCandidateAge(string extractedYear)
        {
            if (!string.IsNullOrEmpty(extractedYear) && extractedYear.Length == 2)
            {
                var yearInTwoDigits = DateTime.Now.Year % 100;
                extractedYear = Convert.ToInt32(extractedYear) > yearInTwoDigits ? "19" + extractedYear : "20" + extractedYear;
            }

            return DateTime.Now.Year - Convert.ToInt32(extractedYear);
        }
    }
}
