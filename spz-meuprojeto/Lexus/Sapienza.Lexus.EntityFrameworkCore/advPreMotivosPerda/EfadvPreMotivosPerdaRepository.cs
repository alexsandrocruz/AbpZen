using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.advPreMotivosPerda;

public class EfadvPreMotivosPerdaRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.advPreMotivosPerda.advPreMotivosPerda, Guid>, 
      IadvPreMotivosPerdaRepository
{
    public EfadvPreMotivosPerdaRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
