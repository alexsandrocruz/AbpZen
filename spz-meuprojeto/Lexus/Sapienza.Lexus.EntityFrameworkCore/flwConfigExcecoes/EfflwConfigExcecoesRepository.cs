using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwConfigExcecoes;

public class EfflwConfigExcecoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid>, 
      IflwConfigExcecoesRepository
{
    public EfflwConfigExcecoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
