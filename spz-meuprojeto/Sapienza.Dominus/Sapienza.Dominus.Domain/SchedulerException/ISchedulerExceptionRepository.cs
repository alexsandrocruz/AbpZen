using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.SchedulerException;

public interface ISchedulerExceptionRepository : IRepository<Sapienza.Dominus.SchedulerException.SchedulerException, Guid>
{
}
