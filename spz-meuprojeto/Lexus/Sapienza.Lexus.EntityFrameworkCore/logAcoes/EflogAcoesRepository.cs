using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.logAcoes;

public class EflogAcoesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.logAcoes.logAcoes, Guid>, 
      IlogAcoesRepository
{
    public EflogAcoesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
