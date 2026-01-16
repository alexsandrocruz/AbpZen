using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreOrigens;

public class EfadvPreOrigensRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advPreOrigens.advPreOrigens, Guid>, 
      IadvPreOrigensRepository
{
    public EfadvPreOrigensRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
