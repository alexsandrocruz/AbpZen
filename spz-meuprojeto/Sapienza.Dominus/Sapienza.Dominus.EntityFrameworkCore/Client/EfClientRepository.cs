using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Client;

public class EfClientRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Client.Client, Guid>, 
      IClientRepository
{
    public EfClientRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
