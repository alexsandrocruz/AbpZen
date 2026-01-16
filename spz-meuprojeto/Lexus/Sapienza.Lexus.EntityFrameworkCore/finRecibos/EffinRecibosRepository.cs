using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finRecibos;

public class EffinRecibosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finRecibos.finRecibos, Guid>, 
      IfinRecibosRepository
{
    public EffinRecibosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
