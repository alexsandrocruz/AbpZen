using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerAvailability;

public interface ISchedulerAvailabilityRepository : IRepository<Sapienza.Dominus.SchedulerAvailability.SchedulerAvailability, Guid>
{
}
