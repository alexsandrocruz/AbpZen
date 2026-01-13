using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.logCampos;

public class EflogCamposRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.logCampos.logCampos, Guid>, 
      IlogCamposRepository
{
    public EflogCamposRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
