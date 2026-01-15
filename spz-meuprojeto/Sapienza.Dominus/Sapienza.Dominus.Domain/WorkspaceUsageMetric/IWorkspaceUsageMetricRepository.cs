using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceUsageMetric;

public interface IWorkspaceUsageMetricRepository : IRepository<Sapienza.Dominus.WorkspaceUsageMetric.WorkspaceUsageMetric, Guid>
{
}
