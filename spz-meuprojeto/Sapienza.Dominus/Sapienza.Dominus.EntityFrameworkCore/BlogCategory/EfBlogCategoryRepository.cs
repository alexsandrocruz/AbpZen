using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogCategory;

public class EfBlogCategoryRepository 
    : EfCoreRepository<DominusDbContext, Dominus.BlogCategory.BlogCategory, Guid>, 
      IBlogCategoryRepository
{
    public EfBlogCategoryRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
