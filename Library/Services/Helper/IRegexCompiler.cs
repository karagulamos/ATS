using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Library.Services.Helper
{
    public interface IRegexCompiler
    {
        Regex Compile(string regexOption);
        List<Regex> GetCompiledRegexes(List<string> regexOptions = null);
    }

    public class RegexOptionHelper
    {
        public const string AgeRegex = "AgeRegex";
        public const string DobRegex = "DobRegex";
        public const string EmailRegex = "EmailRegex";
        public const string FirstLineContainingNameRegex = "FirstLineContainingNameRegex";
        public const string CandidateNameRegex = "CandidateNameRegex";
        public const string PhoneNumberRegex = "PhoneNumberRegex";
        public const string StatOfOriginRegex = "StatOfOriginRegex";
        public const string LastResortDobRegex = "LastResortDobRegex";
    }
}