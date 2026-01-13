using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuPermissoes;

public class EfusuPermissoesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.usuPermissoes.usuPermissoes, Guid>, 
      IusuPermissoesRepository
{
    public EfusuPermissoesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
