using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finCentrosResultado;

public class EffinCentrosResultadoRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finCentrosResultado.finCentrosResultado, Guid>, 
      IfinCentrosResultadoRepository
{
    public EffinCentrosResultadoRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
