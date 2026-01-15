using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Product;

public interface IProductRepository : IRepository<Sapienza.Dominus.Product.Product, Guid>
{
}
