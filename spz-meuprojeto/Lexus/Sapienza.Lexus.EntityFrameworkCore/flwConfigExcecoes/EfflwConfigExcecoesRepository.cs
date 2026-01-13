using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwConfigExcecoes;

public class EfflwConfigExcecoesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.flwConfigExcecoes.flwConfigExcecoes, Guid>, 
      IflwConfigExcecoesRepository
{
    public EfflwConfigExcecoesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
