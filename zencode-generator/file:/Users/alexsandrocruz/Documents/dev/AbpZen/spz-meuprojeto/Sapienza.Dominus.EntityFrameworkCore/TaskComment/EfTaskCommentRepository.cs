using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.TaskComment;

public class EfTaskCommentRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.TaskComment.TaskComment, Guid>, 
      ITaskCommentRepository
{
    public EfTaskCommentRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
