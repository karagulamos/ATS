using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Library.Core.Bootstrapper;
using Library.Core.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    [Export(typeof(ICandidateBuilder))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CandidateResumeValidator : ICandidateBuilder
    {
        [ImportMany]
        private IEnumerable<Lazy<IResumeValidator<Candidate>>> LazyValidators { get; set; }

        private readonly IEnumerable<IResumeValidator<Candidate>> _validators;

        [Import]
        private IRegexCompiler _regexCompiler;

        public CandidateResumeValidator()
        {
            MefDependencyBase.Container.SatisfyImportsOnce(this);
            _validators = LazyValidators.Select(l => l.Value);
        }

        Candidate ICandidateBuilder.BuildFrom(List<string> dataRows)
        {
            var candidate = new Candidate();

            foreach (var resumeValidator in _validators)
            {
                resumeValidator.Validate(candidate, dataRows, _regexCompiler);
            }

            return candidate;
        }
    }
}