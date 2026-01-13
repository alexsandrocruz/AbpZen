using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.opoOportunidades;

public class EfopoOportunidadesRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid>, 
      IopoOportunidadesRepository
{
    public EfopoOportunidadesRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
