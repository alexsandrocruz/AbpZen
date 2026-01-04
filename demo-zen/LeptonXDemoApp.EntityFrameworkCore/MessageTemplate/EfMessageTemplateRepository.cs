using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.MessageTemplate;

public class EfMessageTemplateRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.MessageTemplate.MessageTemplate, Guid>, 
      IMessageTemplateRepository
{
    public EfMessageTemplateRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
