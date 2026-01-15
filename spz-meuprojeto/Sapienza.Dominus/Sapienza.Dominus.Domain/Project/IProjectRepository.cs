using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Project;

public interface IProjectRepository : IRepository<Dominus.Project.Project, Guid>
{
}
