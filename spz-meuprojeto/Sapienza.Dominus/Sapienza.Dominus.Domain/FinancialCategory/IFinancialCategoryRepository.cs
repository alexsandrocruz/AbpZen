using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.FinancialCategory;

public interface IFinancialCategoryRepository : IRepository<Sapienza.Dominus.FinancialCategory.FinancialCategory, Guid>
{
}
