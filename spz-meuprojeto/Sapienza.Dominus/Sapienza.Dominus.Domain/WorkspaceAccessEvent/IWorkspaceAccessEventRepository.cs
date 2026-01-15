using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public interface IWorkspaceAccessEventRepository : IRepository<Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid>
{
}
