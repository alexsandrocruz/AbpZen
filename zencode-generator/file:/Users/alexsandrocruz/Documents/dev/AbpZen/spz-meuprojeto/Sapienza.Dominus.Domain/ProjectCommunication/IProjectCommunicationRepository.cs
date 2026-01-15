using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectCommunication;

public interface IProjectCommunicationRepository : IRepository<Sapienza.Dominus.ProjectCommunication.ProjectCommunication, Guid>
{
}
