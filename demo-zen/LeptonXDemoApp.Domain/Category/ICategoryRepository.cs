using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Category;

public interface ICategoryRepository : IRepository<Category, Guid>
{
}
