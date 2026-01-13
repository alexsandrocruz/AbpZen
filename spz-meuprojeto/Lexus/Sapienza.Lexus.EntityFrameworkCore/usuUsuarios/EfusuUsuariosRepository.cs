using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.usuUsuarios;

public class EfusuUsuariosRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.usuUsuarios.usuUsuarios, Guid>, 
      IusuUsuariosRepository
{
    public EfusuUsuariosRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
