using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadMessageTemplate;

public class EfLeadMessageTemplateRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadMessageTemplate.LeadMessageTemplate, Guid>, 
      ILeadMessageTemplateRepository
{
    public EfLeadMessageTemplateRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
