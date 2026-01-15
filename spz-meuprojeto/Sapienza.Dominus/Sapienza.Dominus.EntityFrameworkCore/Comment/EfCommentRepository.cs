using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Comment;

public class EfCommentRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Comment.Comment, Guid>, 
      ICommentRepository
{
    public EfCommentRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
