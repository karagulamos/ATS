using System.Collections.Generic;
using System.ComponentModel.Composition;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    [InheritedExport]
    public interface IResumeValidator<in T>
    {
        void Validate(T validatedEntity, List<string> dataRows, IRegexCompiler regexCompiler);
    }
}
