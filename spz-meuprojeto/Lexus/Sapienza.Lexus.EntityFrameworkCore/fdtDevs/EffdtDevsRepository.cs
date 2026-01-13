using System;
using Sapienza.Lexus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Lexus.fdtDevs;

public class EffdtDevsRepository 
    : EfCoreRepository<Sapienza.LexusDbContext, Sapienza.Lexus.fdtDevs.fdtDevs, Guid>, 
      IfdtDevsRepository
{
    public EffdtDevsRepository(IDbContextProvider<Sapienza.LexusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
