using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Comment;

public class EfCommentRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Comment.Comment, Guid>, 
      ICommentRepository
{
    public EfCommentRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
