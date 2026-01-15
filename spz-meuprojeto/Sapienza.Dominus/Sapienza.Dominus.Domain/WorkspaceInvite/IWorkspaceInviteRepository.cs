using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.WorkspaceInvite;

public interface IWorkspaceInviteRepository : IRepository<Dominus.WorkspaceInvite.WorkspaceInvite, Guid>
{
}
