using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Core.Bootstrapper;
using Library.Models;
using Library.Services.Common.Extensions;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    class NameValidator : IResumeValidator<Candidate>
    {
        public NameValidator()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
        public void Validate(Candidate candidate, List<string> dataRows, IRegexCompiler regexCompiler)
        {
            var fileLineNameRegex = regexCompiler.Compile(RegexOptionHelper.FirstLineContainingNameRegex);
            var candidateNameRegex = regexCompiler.Compile(RegexOptionHelper.CandidateNameRegex);

            var phrasesToIgnore = ResumeFilterHelper.GetPhrasesNotNames();

            foreach (var dataRow in dataRows)
            {
                if (fileLineNameRegex.IsMatch(dataRow))
                {
                    var firstValidText = fileLineNameRegex.Match(dataRow).Value;
                    var possibleNameTokens = Regex.Replace(firstValidText, @"[^a-z- ]", string.Empty, RegexOptions.IgnoreCase)
                                                  .Split(new []{' ', '\'', '-'}, StringSplitOptions.RemoveEmptyEntries);

                    if (possibleNameTokens.Length > 0 && !possibleNameTokens.Any(name => phrasesToIgnore.Contains(name, new CaseInsensitiveStringComparer())))
                    {
                        MakeCandidateName(candidate, firstValidText.Trim(), true);
                        break;
                    }

                    continue;
                }

                if (candidateNameRegex.IsMatch(dataRow))
                {
                    var matchedName = candidateNameRegex.Match(dataRow).Groups[1].Value;
                    MakeCandidateName(candidate, matchedName.Trim());
                    break;
                }
            }
        }

        private void MakeCandidateName(Candidate candidate, string text, bool isSpecialCase = false)
        {
            if (isSpecialCase)
            {
                var matches = Regex.Matches(text.Trim(), @"(\s+)").OfType<Match>().ToArray();
                
                int maxSpacesBetweenWords = matches.Length > 0 ? matches.Max(m => m.Length) : 1;
                var namesWithSpaces = text.Split(new[] { new string(' ', maxSpacesBetweenWords), }, StringSplitOptions.RemoveEmptyEntries);

                if (namesWithSpaces.Length > 0)
                {
                    var properNames = namesWithSpaces.Select(row => row.Replace(" ", string.Empty)).ToArray();

                    candidate.FirstName = properNames[0].Trim(' ', ',').ToTitleCase();

                    if (properNames.Length > 1)
                        candidate.LastName = properNames[namesWithSpaces.Length - 1].Trim(' ', ',').ToTitleCase();

                    return;
                }
            }

            var names = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (names.Length > 0)
            {
                candidate.FirstName = names[0].Trim(' ', ',').ToTitleCase();
                if (names.Length > 1) candidate.LastName = names[names.Length - 1].Trim(' ', ',').ToTitleCase();
            }
        }

        private class CaseInsensitiveStringComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return y.ToLower().Contains(x);
            }

            public int GetHashCode(string obj)
            {
                return StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj);
            }
        }
    }
}
