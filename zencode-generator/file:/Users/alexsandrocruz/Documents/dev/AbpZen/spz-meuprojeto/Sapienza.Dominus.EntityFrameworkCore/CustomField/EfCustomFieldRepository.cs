using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.CustomField;

public class EfCustomFieldRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.CustomField.CustomField, Guid>, 
      ICustomFieldRepository
{
    public EfCustomFieldRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
