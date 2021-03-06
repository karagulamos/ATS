﻿using System.ComponentModel.Composition;
using System.Linq;
using Library.Core.Models;
using Library.Core.Persistence.Repositories;

namespace Library.Persistence.Repositories
{
    [Export(typeof(ICandidateRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    class CandidateRepository : AbstractDataRepository<Candidate, AtsDbContext>, ICandidateRepository
    {
        public IQueryable<Candidate> GetQueryableCandidates()
        {
            var context = CreateDataContext();
            return context.Candidates.OrderByDescending(c => c.CandidateId);
        }

        public InboundAttachment GetCandidatePath(int candidateId)
        {
            using (var context = CreateDataContext())
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
