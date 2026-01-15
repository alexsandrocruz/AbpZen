using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceInvite;

public class EfWorkspaceInviteRepository 
    : EfCoreRepository<DominusDbContext, Dominus.WorkspaceInvite.WorkspaceInvite, Guid>, 
      IWorkspaceInviteRepository
{
    public EfWorkspaceInviteRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
