using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finCentrosResultado;

public class EffinCentrosResultadoRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.finCentrosResultado.finCentrosResultado, Guid>, 
      IfinCentrosResultadoRepository
{
    public EffinCentrosResultadoRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
