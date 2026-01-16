using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuAcessos;

public class EfusuAcessosRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.usuAcessos.usuAcessos, Guid>, 
      IusuAcessosRepository
{
    public EfusuAcessosRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
