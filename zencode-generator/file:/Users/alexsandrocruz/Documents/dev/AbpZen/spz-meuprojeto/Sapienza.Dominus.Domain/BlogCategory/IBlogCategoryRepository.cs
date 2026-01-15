using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogCategory;

public interface IBlogCategoryRepository : IRepository<Sapienza.Dominus.BlogCategory.BlogCategory, Guid>
{
}
