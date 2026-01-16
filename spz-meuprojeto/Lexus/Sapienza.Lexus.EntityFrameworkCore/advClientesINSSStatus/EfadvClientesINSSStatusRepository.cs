using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesINSSStatus;

public class EfadvClientesINSSStatusRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesINSSStatus.advClientesINSSStatus, Guid>, 
      IadvClientesINSSStatusRepository
{
    public EfadvClientesINSSStatusRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
