using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finLancamentos;

public class EffinLancamentosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finLancamentos.finLancamentos, Guid>, 
      IfinLancamentosRepository
{
    public EffinLancamentosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
