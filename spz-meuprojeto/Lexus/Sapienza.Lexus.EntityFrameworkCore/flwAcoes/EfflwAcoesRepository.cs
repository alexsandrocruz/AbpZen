using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.flwAcoes;

public class EfflwAcoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.flwAcoes.flwAcoes, Guid>, 
      IflwAcoesRepository
{
    public EfflwAcoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
