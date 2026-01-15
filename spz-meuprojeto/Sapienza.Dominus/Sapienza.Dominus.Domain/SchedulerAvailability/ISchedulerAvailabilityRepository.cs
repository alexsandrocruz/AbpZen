using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerAvailability;

public interface ISchedulerAvailabilityRepository : IRepository<Dominus.SchedulerAvailability.SchedulerAvailability, Guid>
{
}
