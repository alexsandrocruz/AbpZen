using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadForm;

public class EfLeadFormRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadForm.LeadForm, Guid>, 
      ILeadFormRepository
{
    public EfLeadFormRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
