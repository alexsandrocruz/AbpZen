using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProposalBlockInstance;

public class EfProposalBlockInstanceRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ProposalBlockInstance.ProposalBlockInstance, Guid>, 
      IProposalBlockInstanceRepository
{
    public EfProposalBlockInstanceRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
