using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuCargos;

public class EfusuCargosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.usuCargos.usuCargos, Guid>, 
      IusuCargosRepository
{
    public EfusuCargosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
