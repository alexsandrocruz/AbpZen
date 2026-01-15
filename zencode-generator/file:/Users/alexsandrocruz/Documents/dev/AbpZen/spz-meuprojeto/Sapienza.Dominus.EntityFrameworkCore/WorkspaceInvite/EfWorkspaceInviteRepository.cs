using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceInvite;

public class EfWorkspaceInviteRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.WorkspaceInvite.WorkspaceInvite, Guid>, 
      IWorkspaceInviteRepository
{
    public EfWorkspaceInviteRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
