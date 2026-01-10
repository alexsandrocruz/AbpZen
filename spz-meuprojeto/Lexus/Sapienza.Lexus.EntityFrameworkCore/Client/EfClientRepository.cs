using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.Client;

public class EfClientRepository 
    : EfCoreRepository<LexusDbContext, Sapienza.Lexus.Client.Client, Guid>, 
      IClientRepository
{
    public EfClientRepository(IDbContextProvider<LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
