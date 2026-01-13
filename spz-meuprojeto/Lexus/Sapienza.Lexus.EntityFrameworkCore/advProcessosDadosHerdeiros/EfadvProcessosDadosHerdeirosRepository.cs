using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessosDadosHerdeiros;

public class EfadvProcessosDadosHerdeirosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advProcessosDadosHerdeiros.advProcessosDadosHerdeiros, Guid>, 
      IadvProcessosDadosHerdeirosRepository
{
    public EfadvProcessosDadosHerdeirosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
