using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advProProbabilidades;

public class EfadvProProbabilidadesRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.advProProbabilidades.advProProbabilidades, Guid>, 
      IadvProProbabilidadesRepository
{
    public EfadvProProbabilidadesRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
