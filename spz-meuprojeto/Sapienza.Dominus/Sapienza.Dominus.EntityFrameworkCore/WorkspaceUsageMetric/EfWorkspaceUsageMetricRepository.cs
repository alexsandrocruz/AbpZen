using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

public class EfWorkspaceUsageMetricRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, Guid>, 
      IWorkspaceUsageMetricRepository
{
    public EfWorkspaceUsageMetricRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
