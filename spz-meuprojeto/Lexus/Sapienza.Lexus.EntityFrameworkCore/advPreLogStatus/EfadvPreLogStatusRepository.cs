using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreLogStatus;

public class EfadvPreLogStatusRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreLogStatus.advPreLogStatus, Guid>, 
      IadvPreLogStatusRepository
{
    public EfadvPreLogStatusRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
