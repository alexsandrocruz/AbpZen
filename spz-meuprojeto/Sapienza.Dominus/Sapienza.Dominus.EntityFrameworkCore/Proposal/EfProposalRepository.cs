using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Proposal;

public class EfProposalRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Proposal.Proposal, Guid>, 
      IProposalRepository
{
    public EfProposalRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
