using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabLembretes;

public class EffabLembretesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabLembretes.fabLembretes, Guid>, 
      IfabLembretesRepository
{
    public EffabLembretesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
