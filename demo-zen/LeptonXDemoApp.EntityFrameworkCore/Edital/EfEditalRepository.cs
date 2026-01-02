using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Edital;

public class EfEditalRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, Edital, Guid>, 
      IEditalRepository
{
    public EfEditalRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
