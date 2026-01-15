using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadTag;

public class EfLeadTagRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadTag.LeadTag, Guid>, 
      ILeadTagRepository
{
    public EfLeadTagRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
