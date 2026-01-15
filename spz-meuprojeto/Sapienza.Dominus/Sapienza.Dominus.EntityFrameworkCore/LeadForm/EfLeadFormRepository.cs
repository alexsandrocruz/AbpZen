using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadForm;

public class EfLeadFormRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadForm.LeadForm, Guid>, 
      ILeadFormRepository
{
    public EfLeadFormRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
