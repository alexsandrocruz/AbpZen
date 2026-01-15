using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogPostVersion;

public class EfBlogPostVersionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.BlogPostVersion.BlogPostVersion, Guid>, 
      IBlogPostVersionRepository
{
    public EfBlogPostVersionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
