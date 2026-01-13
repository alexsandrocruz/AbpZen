using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreStatus;

public class EfadvPreStatusRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreStatus.advPreStatus, Guid>, 
      IadvPreStatusRepository
{
    public EfadvPreStatusRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
