using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreCheckLists;

public class EfadvPreCheckListsRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreCheckLists.advPreCheckLists, Guid>, 
      IadvPreCheckListsRepository
{
    public EfadvPreCheckListsRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
