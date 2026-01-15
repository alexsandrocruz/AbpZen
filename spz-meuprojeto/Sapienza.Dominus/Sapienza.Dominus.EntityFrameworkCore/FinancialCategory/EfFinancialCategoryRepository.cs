using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.FinancialCategory;

public class EfFinancialCategoryRepository 
    : EfCoreRepository<DominusDbContext, Dominus.FinancialCategory.FinancialCategory, Guid>, 
      IFinancialCategoryRepository
{
    public EfFinancialCategoryRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
