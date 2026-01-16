using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

public class EfadvProcessosDadosHerdeirosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, Guid>, 
      IadvProcessosDadosHerdeirosRepository
{
    public EfadvProcessosDadosHerdeirosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
