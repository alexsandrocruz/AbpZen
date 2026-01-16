using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliLocaisAtendido;

public class EfadvCliLocaisAtendidoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, Guid>, 
      IadvCliLocaisAtendidoRepository
{
    public EfadvCliLocaisAtendidoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
