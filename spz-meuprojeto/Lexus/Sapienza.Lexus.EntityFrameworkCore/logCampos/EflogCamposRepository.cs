using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.logCampos;

public class EflogCamposRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.logCampos.logCampos, Guid>, 
      IlogCamposRepository
{
    public EflogCamposRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
