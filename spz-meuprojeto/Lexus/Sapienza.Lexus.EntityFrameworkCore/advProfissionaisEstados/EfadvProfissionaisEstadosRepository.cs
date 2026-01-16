using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProfissionaisEstados;

public class EfadvProfissionaisEstadosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProfissionaisEstados.advProfissionaisEstados, Guid>, 
      IadvProfissionaisEstadosRepository
{
    public EfadvProfissionaisEstadosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
