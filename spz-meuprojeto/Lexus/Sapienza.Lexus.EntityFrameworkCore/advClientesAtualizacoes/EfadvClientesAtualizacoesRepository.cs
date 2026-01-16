using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advClientesAtualizacoes;

public class EfadvClientesAtualizacoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, Guid>, 
      IadvClientesAtualizacoesRepository
{
    public EfadvClientesAtualizacoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
