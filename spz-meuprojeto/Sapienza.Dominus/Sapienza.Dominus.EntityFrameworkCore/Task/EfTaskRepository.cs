using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Task;

public class EfTaskRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Task.Task, Guid>, 
      ITaskRepository
{
    public EfTaskRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
