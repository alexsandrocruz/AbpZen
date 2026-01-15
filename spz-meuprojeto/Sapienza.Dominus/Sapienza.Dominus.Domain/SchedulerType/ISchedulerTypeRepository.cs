using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerType;

public interface ISchedulerTypeRepository : IRepository<Dominus.SchedulerType.SchedulerType, Guid>
{
}
