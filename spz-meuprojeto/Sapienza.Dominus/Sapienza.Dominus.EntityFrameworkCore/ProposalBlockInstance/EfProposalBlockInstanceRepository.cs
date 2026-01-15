using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProposalBlockInstance;

public class EfProposalBlockInstanceRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ProposalBlockInstance.ProposalBlockInstance, Guid>, 
      IProposalBlockInstanceRepository
{
    public EfProposalBlockInstanceRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
