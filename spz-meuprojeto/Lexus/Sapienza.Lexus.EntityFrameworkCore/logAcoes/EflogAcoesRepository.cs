using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.logAcoes;

public class EflogAcoesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.logAcoes.logAcoes, Guid>, 
      IlogAcoesRepository
{
    public EflogAcoesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
