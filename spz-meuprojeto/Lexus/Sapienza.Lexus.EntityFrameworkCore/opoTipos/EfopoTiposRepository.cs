using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoTipos;

public class EfopoTiposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.opoTipos.opoTipos, Guid>, 
      IopoTiposRepository
{
    public EfopoTiposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
