using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerType;

public interface ISchedulerTypeRepository : IRepository<Sapienza.Dominus.SchedulerType.SchedulerType, Guid>
{
}
