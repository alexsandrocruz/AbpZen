using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Task;

public interface ITaskRepository : IRepository<Sapienza.Dominus.Task.Task, Guid>
{
}
