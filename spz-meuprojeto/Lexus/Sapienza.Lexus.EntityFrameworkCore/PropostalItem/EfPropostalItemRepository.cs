using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.PropostalItem;

public class EfPropostalItemRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.PropostalItem.PropostalItem, Guid>, 
      IPropostalItemRepository
{
    public EfPropostalItemRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
