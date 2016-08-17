using System.Collections.Generic;
using System.ComponentModel.Composition;
using Library.Core.Bootstrapper;
using Library.Core.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    class EmailValidator : IResumeValidator<Candidate>
    {
       public EmailValidator()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
        }
        public void Validate(Candidate candidate, List<string> dataRows, IRegexCompiler regexCompiler)
        {
            var emailRegex = regexCompiler.Compile(RegexOptionHelper.EmailRegex);

            foreach (var dataRow in dataRows)
            {
                if (emailRegex.IsMatch(dataRow))
                {
                    var matchedGroup = emailRegex.Match(dataRow);
                    candidate.Email = matchedGroup.Value.Trim();

                    break;
                }
            }
        }
    }
}
