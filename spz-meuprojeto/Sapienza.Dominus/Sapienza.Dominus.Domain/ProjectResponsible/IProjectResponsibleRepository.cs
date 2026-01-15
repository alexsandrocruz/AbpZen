using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectResponsible;

public interface IProjectResponsibleRepository : IRepository<Dominus.ProjectResponsible.ProjectResponsible, Guid>
{
}
