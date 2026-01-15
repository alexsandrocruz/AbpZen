using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadFormField;

public class EfLeadFormFieldRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadFormField.LeadFormField, Guid>, 
      ILeadFormFieldRepository
{
    public EfLeadFormFieldRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
