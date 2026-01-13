using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuCargos;

public class EfusuCargosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.usuCargos.usuCargos, Guid>, 
      IusuCargosRepository
{
    public EfusuCargosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
