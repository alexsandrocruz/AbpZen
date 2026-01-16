using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreStatus;

public class EfadvPreStatusRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreStatus.advPreStatus, Guid>, 
      IadvPreStatusRepository
{
    public EfadvPreStatusRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
