using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadMessageTemplate;

public class EfLeadMessageTemplateRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadMessageTemplate.LeadMessageTemplate, Guid>, 
      ILeadMessageTemplateRepository
{
    public EfLeadMessageTemplateRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
