using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogPost;

public class EfBlogPostRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.BlogPost.BlogPost, Guid>, 
      IBlogPostRepository
{
    public EfBlogPostRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
