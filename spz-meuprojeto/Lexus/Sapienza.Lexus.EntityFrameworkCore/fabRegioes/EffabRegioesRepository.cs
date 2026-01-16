using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabRegioes;

public class EffabRegioesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.fabRegioes.fabRegioes, Guid>, 
      IfabRegioesRepository
{
    public EffabRegioesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
