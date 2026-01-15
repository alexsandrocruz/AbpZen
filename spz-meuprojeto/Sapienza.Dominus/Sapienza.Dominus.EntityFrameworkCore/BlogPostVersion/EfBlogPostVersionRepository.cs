using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogPostVersion;

public class EfBlogPostVersionRepository 
    : EfCoreRepository<DominusDbContext, Dominus.BlogPostVersion.BlogPostVersion, Guid>, 
      IBlogPostVersionRepository
{
    public EfBlogPostVersionRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
