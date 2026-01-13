using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreCheckListsGrupos;

public class EfadvPreCheckListsGruposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreCheckListsGrupos.advPreCheckListsGrupos, Guid>, 
      IadvPreCheckListsGruposRepository
{
    public EfadvPreCheckListsGruposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
