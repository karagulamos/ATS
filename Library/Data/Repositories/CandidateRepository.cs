using System.ComponentModel.Composition;
using System.Linq;
using Library.Models;

namespace Library.Data.Repositories
{
    [Export(typeof(ICandidateRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class CandidateRepository : AbstractDataManager<Candidate, AtsDbContext>, ICandidateRepository
    {
        public IQueryable<Candidate> GetQueryableCandidates()
        {
            var context = new AtsDbContext();
            return context.Candidates.OrderByDescending(c => c.CandidateId);
        }

        public InboundAttachment GetCandidatePath(int candidateId)
        {
            using (var context = new AtsDbContext())
            {
                var result = from candidate in context.Candidates
                    join attachment in context.InboundAttachments on candidate.InboundAttachmentId equals attachment.InboundAttachmentId
                    where candidate.CandidateId == candidateId
                    select attachment;

                return result.SingleOrDefault();
            }
        }
    }
}
