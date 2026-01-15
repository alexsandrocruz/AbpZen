using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TaskComment;

public class EfTaskCommentRepository 
    : EfCoreRepository<DominusDbContext, Dominus.TaskComment.TaskComment, Guid>, 
      ITaskCommentRepository
{
    public EfTaskCommentRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
