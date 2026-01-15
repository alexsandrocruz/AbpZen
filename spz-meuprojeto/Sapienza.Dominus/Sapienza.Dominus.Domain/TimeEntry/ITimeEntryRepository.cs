using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TimeEntry;

public interface ITimeEntryRepository : IRepository<Dominus.TimeEntry.TimeEntry, Guid>
{
}
