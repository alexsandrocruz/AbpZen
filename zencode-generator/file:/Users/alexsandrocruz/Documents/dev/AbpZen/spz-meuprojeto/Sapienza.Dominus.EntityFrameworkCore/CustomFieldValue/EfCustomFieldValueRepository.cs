using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.CustomFieldValue;

public class EfCustomFieldValueRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.CustomFieldValue.CustomFieldValue, Guid>, 
      ICustomFieldValueRepository
{
    public EfCustomFieldValueRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
