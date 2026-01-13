using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fabMotivosPerda;

public class EffabMotivosPerdaRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fabMotivosPerda.fabMotivosPerda, Guid>, 
      IfabMotivosPerdaRepository
{
    public EffabMotivosPerdaRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
