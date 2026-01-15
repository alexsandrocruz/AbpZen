using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadFormField;

public class EfLeadFormFieldRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadFormField.LeadFormField, Guid>, 
      ILeadFormFieldRepository
{
    public EfLeadFormFieldRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
