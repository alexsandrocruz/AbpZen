using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRecibos;

public class EffinRecibosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finRecibos.finRecibos, Guid>, 
      IfinRecibosRepository
{
    public EffinRecibosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
