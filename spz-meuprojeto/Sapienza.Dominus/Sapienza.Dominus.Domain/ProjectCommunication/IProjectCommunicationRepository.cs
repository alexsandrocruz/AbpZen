using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectCommunication;

public interface IProjectCommunicationRepository : IRepository<Dominus.ProjectCommunication.ProjectCommunication, Guid>
{
}
