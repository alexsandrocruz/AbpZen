using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreProcessosCheckLists;

public class EfadvPreProcessosCheckListsRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreProcessosCheckLists.advPreProcessosCheckLists, Guid>, 
      IadvPreProcessosCheckListsRepository
{
    public EfadvPreProcessosCheckListsRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
