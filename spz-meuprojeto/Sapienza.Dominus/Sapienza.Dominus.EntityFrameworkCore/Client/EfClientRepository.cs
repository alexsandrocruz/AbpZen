using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Client;

public class EfClientRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Client.Client, Guid>, 
      IClientRepository
{
    public EfClientRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
