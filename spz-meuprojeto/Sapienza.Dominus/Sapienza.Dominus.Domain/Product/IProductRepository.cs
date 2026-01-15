using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Product;

public interface IProductRepository : IRepository<Dominus.Product.Product, Guid>
{
}
