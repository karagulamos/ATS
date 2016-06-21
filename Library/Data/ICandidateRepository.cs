using System.Linq;
using Library.Data.Managers;
using Library.Models;

namespace Library.Data
{
    public interface ICandidateRepository : IDataManager<Candidate>
    {
        IQueryable<Candidate> GetQueryableCandidates();
        InboundAttachment GetCandidatePath(int candidateId);
    }
}