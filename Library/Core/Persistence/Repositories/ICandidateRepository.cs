using System.Linq;
using Library.Core.Models;

namespace Library.Core.Persistence.Repositories
{
    public interface ICandidateRepository : IDataRepository<Candidate>
    {
        IQueryable<Candidate> GetQueryableCandidates();
        InboundAttachment GetCandidatePath(int candidateId);
    }
}