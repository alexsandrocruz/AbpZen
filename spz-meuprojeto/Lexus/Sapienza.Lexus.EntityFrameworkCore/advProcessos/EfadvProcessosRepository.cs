using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProcessos;

public class EfadvProcessosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProcessos.advProcessos, Guid>, 
      IadvProcessosRepository
{
    public EfadvProcessosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
