using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.WorkspaceAccessEvent;

public class EfWorkspaceAccessEventRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.WorkspaceAccessEvent.WorkspaceAccessEvent, Guid>, 
      IWorkspaceAccessEventRepository
{
    public EfWorkspaceAccessEventRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
