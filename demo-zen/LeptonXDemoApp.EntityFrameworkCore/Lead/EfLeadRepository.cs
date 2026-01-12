using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Lead;

public class EfLeadRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.Lead.Lead, Guid>, 
      ILeadRepository
{
    public EfLeadRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
