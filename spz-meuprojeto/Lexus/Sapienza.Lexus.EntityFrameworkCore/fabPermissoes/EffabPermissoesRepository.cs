using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabPermissoes;

public class EffabPermissoesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabPermissoes.fabPermissoes, Guid>, 
      IfabPermissoesRepository
{
    public EffabPermissoesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
