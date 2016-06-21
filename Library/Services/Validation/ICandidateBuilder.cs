using System.Collections.Generic;
using Library.Models;
using Library.Services.Helper;

namespace Library.Services.Validation
{
    public interface ICandidateBuilder
    {
        Candidate BuildFrom(List<string> dataRows);
    }
}