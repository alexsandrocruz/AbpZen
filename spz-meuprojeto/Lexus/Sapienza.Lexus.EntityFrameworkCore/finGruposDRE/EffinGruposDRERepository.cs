using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.finGruposDRE;

public class EffinGruposDRERepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.finGruposDRE.finGruposDRE, Guid>, 
      IfinGruposDRERepository
{
    public EffinGruposDRERepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
