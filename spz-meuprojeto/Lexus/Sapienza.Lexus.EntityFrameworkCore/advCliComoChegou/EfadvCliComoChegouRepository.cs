using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliComoChegou;

public class EfadvCliComoChegouRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliComoChegou.advCliComoChegou, Guid>, 
      IadvCliComoChegouRepository
{
    public EfadvCliComoChegouRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
