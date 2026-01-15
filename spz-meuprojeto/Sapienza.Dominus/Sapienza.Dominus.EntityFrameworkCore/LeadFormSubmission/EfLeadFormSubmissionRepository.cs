using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadFormSubmission;

public class EfLeadFormSubmissionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.LeadFormSubmission.LeadFormSubmission, Guid>, 
      ILeadFormSubmissionRepository
{
    public EfLeadFormSubmissionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
