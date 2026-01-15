using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProposalItem;

public class EfProposalItemRepository 
    : EfCoreRepository<DominusDbContext, Dominus.ProposalItem.ProposalItem, Guid>, 
      IProposalItemRepository
{
    public EfProposalItemRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
