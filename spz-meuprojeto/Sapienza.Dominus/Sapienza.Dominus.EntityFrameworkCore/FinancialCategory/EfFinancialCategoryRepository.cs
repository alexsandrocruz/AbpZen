using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.FinancialCategory;

public class EfFinancialCategoryRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.FinancialCategory.FinancialCategory, Guid>, 
      IFinancialCategoryRepository
{
    public EfFinancialCategoryRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
