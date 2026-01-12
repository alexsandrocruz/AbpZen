using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.LeadMessage;

public class EfLeadMessageRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.LeadMessage.LeadMessage, Guid>, 
      ILeadMessageRepository
{
    public EfLeadMessageRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
