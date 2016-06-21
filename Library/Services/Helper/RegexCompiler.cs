using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Core.Bootstrapper;

namespace Library.Services.Helper
{
    [Export(typeof(IRegexCompiler))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class RegexCompiler : IRegexCompiler
    {
        private const string AgeRegexPattern = @"^\s*age[^\w\d]*(\d{4}|\d{2})\s*$";
        private const string DobRegexPattern = @"^[a-z ]*[^\d\w]*(date\s*of\s*birth|d[. ]*?o[. ]*?b|age)[^\w\d]+([\w, ]+[^\d](\d{4})|\d{2}[-/.~_ ]+\d{2}[-/.~_ ]+(\d{2}|\d{4})[^\d]*)[^\d]*$";
        private const string LastResortDobRegexPattern = @"^\s*\d{2}[-/.~_ ]+\d{2}[-/.~_ ]+(\d{2}|\d{4})[^\d]*\s*$";

        private const string EmailRegexPattern = @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";

        private const string FirstLineContainingNameRegexPattern = @"(?=\s*[a-z])(^\s*([a-z\- ]+[,]?\s+[a-z\- ]+)\s*$)";
        private const string CandidateNameRegexPattern = @"^\s*name[^\w\d]+?([\w\s-',]+)";

        private const string PhoneNumberRegexPattern = 
@"(\+?234[ ]*[7-9]\d{9}|\s*0\s*[7-9]\d{9}|\+?234\s*[\(\-]\s*0?\s*[\-\)]?\s*[7-9]\d{9}|\s*\+?234\s*[-.( ]?\s*0?\d{3}\s*[-.) ]?\s*\d{3}\s*[-.( ]?\s*\d{4}|\s*0?\d{3}\s*[-.) ]?\s*\d{3}\s*[-. ]?\s*\d{4})";

        //private const string StatOfOriginRegexPattern = @"^\s*(origin.*|state|state.*origin|place.*birth|birth.*place|nationality)[^\w\d]*([a-z-\s]+)\s*$";
        private const string StatOfOriginRegexPattern = @"^.*?\s*(origin.*|state.*origin|state|place.*birth|birth.*place|nationality)[\s:\-]+?([a-z-\s]+)\S+?(state)?.*$";

        public readonly IDictionary<string, Regex> RegexMap;
        public RegexCompiler()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);

            RegexMap = new Dictionary<string, Regex>
            {
                {RegexOptionHelper.AgeRegex, Build(AgeRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.DobRegex, Build(DobRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.LastResortDobRegex, Build(LastResortDobRegexPattern, RegexOptions.IgnoreCase)},

                {RegexOptionHelper.EmailRegex, Build(EmailRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.FirstLineContainingNameRegex, Build(FirstLineContainingNameRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.CandidateNameRegex, Build(CandidateNameRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.PhoneNumberRegex, Build(PhoneNumberRegexPattern, RegexOptions.IgnoreCase)},
                {RegexOptionHelper.StatOfOriginRegex, Build(StatOfOriginRegexPattern, RegexOptions.IgnoreCase)},
            };
        }

        public Regex Compile(string regexOption)
        {
            return RegexMap[regexOption];
        }

        public List<Regex> GetCompiledRegexes(List<string> regexOptions)
        {
            if (regexOptions == null)
                return RegexMap.Values.ToList();

            return regexOptions.Select(regexOption => RegexMap[regexOption]).ToList();
        }

        private Regex Build(string regexPattern, RegexOptions regexOptions = RegexOptions.None)
        {
            return new Regex(regexPattern, RegexOptions.Compiled | regexOptions);
        }
    }
}
