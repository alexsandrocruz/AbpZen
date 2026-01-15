using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Budget;

public class EfBudgetRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Budget.Budget, Guid>, 
      IBudgetRepository
{
    public EfBudgetRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
