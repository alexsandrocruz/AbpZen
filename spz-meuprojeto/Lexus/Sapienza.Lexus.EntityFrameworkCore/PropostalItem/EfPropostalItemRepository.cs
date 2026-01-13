using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.PropostalItem;

public class EfPropostalItemRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.PropostalItem.PropostalItem, Guid>, 
      IPropostalItemRepository
{
    public EfPropostalItemRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
