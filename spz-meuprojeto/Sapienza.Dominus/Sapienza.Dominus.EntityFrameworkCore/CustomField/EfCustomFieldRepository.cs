using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.CustomField;

public class EfCustomFieldRepository 
    : EfCoreRepository<DominusDbContext, Dominus.CustomField.CustomField, Guid>, 
      ICustomFieldRepository
{
    public EfCustomFieldRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
