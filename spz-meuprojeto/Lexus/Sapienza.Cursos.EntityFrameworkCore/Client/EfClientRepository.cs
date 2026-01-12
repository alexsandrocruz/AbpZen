using System;
using Sapienza.Cursos.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Cursos.Client;

public class EfClientRepository 
    : EfCoreRepository<Sapienza.CursosDbContext, Sapienza.Cursos.Client.Client, Guid>, 
      IClientRepository
{
    public EfClientRepository(IDbContextProvider<Sapienza.CursosDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
