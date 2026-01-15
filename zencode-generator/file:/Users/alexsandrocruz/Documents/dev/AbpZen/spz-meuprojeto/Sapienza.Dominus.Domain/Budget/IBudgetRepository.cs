using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Budget;

public interface IBudgetRepository : IRepository<Sapienza.Dominus.Budget.Budget, Guid>
{
}
