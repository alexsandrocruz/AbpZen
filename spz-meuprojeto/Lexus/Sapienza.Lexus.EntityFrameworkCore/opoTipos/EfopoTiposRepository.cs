using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoTipos;

public class EfopoTiposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.opoTipos.opoTipos, Guid>, 
      IopoTiposRepository
{
    public EfopoTiposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
