using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.LeadFormSubmission;

public class EfLeadFormSubmissionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.LeadFormSubmission.LeadFormSubmission, Guid>, 
      ILeadFormSubmissionRepository
{
    public EfLeadFormSubmissionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
