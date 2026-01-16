using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finExtrato;

public class EffinExtratoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finExtrato.finExtrato, Guid>, 
      IfinExtratoRepository
{
    public EffinExtratoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
