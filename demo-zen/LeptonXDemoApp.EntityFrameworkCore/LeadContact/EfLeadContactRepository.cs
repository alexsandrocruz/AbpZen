using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.LeadContact;

public class EfLeadContactRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.LeadContact.LeadContact, Guid>, 
      ILeadContactRepository
{
    public EfLeadContactRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
