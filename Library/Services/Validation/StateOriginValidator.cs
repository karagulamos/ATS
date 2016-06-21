using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    class StateOriginValidator : IResumeValidator<Candidate>
    {
        public void Validate(Candidate candidate, List<string> dataRows, IRegexCompiler regexCompiler)
        {
            var states = ResumeFilterHelper.GetStates();

            var stateOfOriginRegex = regexCompiler.Compile(RegexOptionHelper.StatOfOriginRegex);
            var wordsToIgnore = ResumeFilterHelper.GetStopWords();

            foreach (var dataRow in dataRows)
            {

                if (stateOfOriginRegex.IsMatch(dataRow))
                {
                    var stateRow = dataRow.Trim().ToLower();
                    var possibleStateTokens = Regex.Replace(stateRow, @"[^a-z-,: ]", string.Empty, RegexOptions.IgnoreCase)
                                                   .Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    if (possibleStateTokens.All(state => !wordsToIgnore.Contains(state, new StateComparer())))
                    {
                        var foundState =
                            possibleStateTokens.FirstOrDefault(state => states.Contains(state, new StateComparer()));

                        if (!string.IsNullOrEmpty(foundState))
                        {
                            candidate.StateOfOrigin = states.First(state => state.ToLower().Contains(foundState));
                            return;
                        }
                    }
                }

                var trimmedStateRow = dataRow.Trim().ToLower();
                if (trimmedStateRow.Contains("origin") || trimmedStateRow.Contains("state") && trimmedStateRow.Contains("origin") || trimmedStateRow.Contains("nationality"))
                {
                    var possibleStateTokens = Regex.Replace(trimmedStateRow, @"[^a-z-,: ]", string.Empty, RegexOptions.IgnoreCase)
                                                   .Split(new[] { ' ', '-', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    var foundState = possibleStateTokens.FirstOrDefault(state => states.Contains(state, new StateComparer()));

                    if (!string.IsNullOrEmpty(foundState))
                    {
                        candidate.StateOfOrigin = states.First(state => state.ToLower().Contains(foundState));
                        return;
                    }
                }
            }

            HashSet<string> wordsInRow = new HashSet<string>(dataRows.SelectMany(row => row.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)));
            var candidateState = states.FirstOrDefault(state => wordsInRow.Contains(state.ToLower()));

            if (candidateState != null)
                candidate.StateOfOrigin = candidateState;
        }

        public class StateComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                var stateTokens = x.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                return stateTokens.Any(token => token.ToLower() == y);
            }

            public int GetHashCode(string obj)
            {
                return StringComparer.CurrentCultureIgnoreCase.GetHashCode(obj);
            }
        }
    }
}
