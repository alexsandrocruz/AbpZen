using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProOrgaos;

public class EfadvProOrgaosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProOrgaos.advProOrgaos, Guid>, 
      IadvProOrgaosRepository
{
    public EfadvProOrgaosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
