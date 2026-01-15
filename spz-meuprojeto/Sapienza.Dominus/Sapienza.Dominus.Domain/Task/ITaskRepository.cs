using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Task;

public interface ITaskRepository : IRepository<Dominus.Task.Task, Guid>
{
}
