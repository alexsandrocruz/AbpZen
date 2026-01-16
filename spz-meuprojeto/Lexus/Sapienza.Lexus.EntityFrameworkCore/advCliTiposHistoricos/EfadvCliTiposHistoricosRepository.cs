using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliTiposHistoricos;

public class EfadvCliTiposHistoricosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, Guid>, 
      IadvCliTiposHistoricosRepository
{
    public EfadvCliTiposHistoricosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
