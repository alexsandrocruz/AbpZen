using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliComoChegou;

public class EfadvCliComoChegouRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliComoChegou.advCliComoChegou, Guid>, 
      IadvCliComoChegouRepository
{
    public EfadvCliComoChegouRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
