using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public class EfWorkspaceAccessEventRepository 
    : EfCoreRepository<DominusDbContext, Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid>, 
      IWorkspaceAccessEventRepository
{
    public EfWorkspaceAccessEventRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
