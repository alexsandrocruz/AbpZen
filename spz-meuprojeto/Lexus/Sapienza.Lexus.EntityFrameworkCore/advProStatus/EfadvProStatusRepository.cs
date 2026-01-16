using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProStatus;

public class EfadvProStatusRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProStatus.advProStatus, Guid>, 
      IadvProStatusRepository
{
    public EfadvProStatusRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
