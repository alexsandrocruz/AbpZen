using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.CustomFieldValue;

public class EfCustomFieldValueRepository 
    : EfCoreRepository<DominusDbContext, Dominus.CustomFieldValue.CustomFieldValue, Guid>, 
      ICustomFieldValueRepository
{
    public EfCustomFieldValueRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
