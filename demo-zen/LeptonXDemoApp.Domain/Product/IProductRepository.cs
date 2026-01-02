using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Product;

public interface IProductRepository : IRepository<Product, Guid>
{
}
