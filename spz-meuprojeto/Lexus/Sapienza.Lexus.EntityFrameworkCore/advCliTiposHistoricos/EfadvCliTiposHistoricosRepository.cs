using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advCliTiposHistoricos;

public class EfadvCliTiposHistoricosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advCliTiposHistoricos.advCliTiposHistoricos, Guid>, 
      IadvCliTiposHistoricosRepository
{
    public EfadvCliTiposHistoricosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
