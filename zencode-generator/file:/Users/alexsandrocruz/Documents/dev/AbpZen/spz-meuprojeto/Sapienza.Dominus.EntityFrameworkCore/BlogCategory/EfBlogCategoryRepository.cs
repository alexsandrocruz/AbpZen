using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.BlogCategory;

public class EfBlogCategoryRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.BlogCategory.BlogCategory, Guid>, 
      IBlogCategoryRepository
{
    public EfBlogCategoryRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
