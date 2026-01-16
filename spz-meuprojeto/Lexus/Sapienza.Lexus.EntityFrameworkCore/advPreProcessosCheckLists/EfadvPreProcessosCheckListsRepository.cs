using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

public class EfadvPreProcessosCheckListsRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, Guid>, 
      IadvPreProcessosCheckListsRepository
{
    public EfadvPreProcessosCheckListsRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
