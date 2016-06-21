using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Core.Bootstrapper;

namespace Library.Services.Helper
{
    [Export(typeof(IPatternMatcher))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class ResumePatternMatcher : IPatternMatcher
    {
        [Import]
        private IRegexCompiler _regexCompiler;

        private readonly List<Regex> _regexes;
        public ResumePatternMatcher()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);

            _regexes = _regexCompiler.GetCompiledRegexes(new List<string>
            {
                RegexOptionHelper.EmailRegex,
                RegexOptionHelper.PhoneNumberRegex,
                RegexOptionHelper.StatOfOriginRegex,
                RegexOptionHelper.DobRegex,
                RegexOptionHelper.AgeRegex,
                RegexOptionHelper.LastResortDobRegex
            });
        }

        public IEnumerable<string> GetMatchedRows(List<string> itemsToScan)
        {
            var matchedRows = new HashSet<string>();

            foreach (var regex in _regexes)
            {
                matchedRows.UnionWith(itemsToScan.Where(row => regex.IsMatch(row)));
            }

            return matchedRows;
        }
    }
}
