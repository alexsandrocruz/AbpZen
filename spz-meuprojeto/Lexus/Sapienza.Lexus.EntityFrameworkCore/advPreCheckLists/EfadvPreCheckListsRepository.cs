using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreCheckLists;

public class EfadvPreCheckListsRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreCheckLists.advPreCheckLists, Guid>, 
      IadvPreCheckListsRepository
{
    public EfadvPreCheckListsRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
