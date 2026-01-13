using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliLocaisAtendido;

public class EfadvCliLocaisAtendidoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliLocaisAtendido.advCliLocaisAtendido, Guid>, 
      IadvCliLocaisAtendidoRepository
{
    public EfadvCliLocaisAtendidoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
