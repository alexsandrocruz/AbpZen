using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public interface IWorkspaceAccessEventRepository : IRepository<Sapienza.Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid>
{
}
