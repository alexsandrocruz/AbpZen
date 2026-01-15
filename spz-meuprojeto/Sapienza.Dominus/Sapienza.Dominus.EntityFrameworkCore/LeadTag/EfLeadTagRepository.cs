using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadTag;

public class EfLeadTagRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadTag.LeadTag, Guid>, 
      ILeadTagRepository
{
    public EfLeadTagRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
