using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advAgeTiposTarefas;

public class EfadvAgeTiposTarefasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advAgeTiposTarefas.advAgeTiposTarefas, Guid>, 
      IadvAgeTiposTarefasRepository
{
    public EfadvAgeTiposTarefasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
