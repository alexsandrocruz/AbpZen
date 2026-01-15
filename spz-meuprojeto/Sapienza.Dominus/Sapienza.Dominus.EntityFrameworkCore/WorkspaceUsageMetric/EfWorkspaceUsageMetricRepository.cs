using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

public class EfWorkspaceUsageMetricRepository 
    : EfCoreRepository<DominusDbContext, Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, Guid>, 
      IWorkspaceUsageMetricRepository
{
    public EfWorkspaceUsageMetricRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
