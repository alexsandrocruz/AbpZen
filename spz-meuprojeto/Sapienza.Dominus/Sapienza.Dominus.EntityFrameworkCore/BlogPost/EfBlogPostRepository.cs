using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogPost;

public class EfBlogPostRepository 
    : EfCoreRepository<DominusDbContext, Dominus.BlogPost.BlogPost, Guid>, 
      IBlogPostRepository
{
    public EfBlogPostRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
