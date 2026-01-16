using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPlanoContas;

public class EffinPlanoContasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finPlanoContas.finPlanoContas, Guid>, 
      IfinPlanoContasRepository
{
    public EffinPlanoContasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
