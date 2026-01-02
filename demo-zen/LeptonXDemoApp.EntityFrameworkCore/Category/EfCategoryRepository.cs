using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Category;

public class EfCategoryRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, Category, Guid>, 
      ICategoryRepository
{
    public EfCategoryRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
