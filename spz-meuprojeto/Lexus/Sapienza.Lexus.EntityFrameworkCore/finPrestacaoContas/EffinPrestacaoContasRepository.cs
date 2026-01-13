using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finPrestacaoContas;

public class EffinPrestacaoContasRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid>, 
      IfinPrestacaoContasRepository
{
    public EffinPrestacaoContasRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
