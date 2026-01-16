using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advAgeTiposTarefas;

public class EfadvAgeTiposTarefasRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, Guid>, 
      IadvAgeTiposTarefasRepository
{
    public EfadvAgeTiposTarefasRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
