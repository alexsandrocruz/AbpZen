using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectFollower;

public interface IProjectFollowerRepository : IRepository<Dominus.ProjectFollower.ProjectFollower, Guid>
{
}
