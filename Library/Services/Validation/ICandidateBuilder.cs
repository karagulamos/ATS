using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Services.Validation
{
    public interface ICandidateBuilder
    {
        Candidate BuildFrom(List<string> dataRows);
    }
}