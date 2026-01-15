using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.ProposalItem;

public class EfProposalItemRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.ProposalItem.ProposalItem, Guid>, 
      IProposalItemRepository
{
    public EfProposalItemRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
