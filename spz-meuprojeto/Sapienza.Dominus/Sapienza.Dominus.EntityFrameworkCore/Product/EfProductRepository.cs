using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Product;

public class EfProductRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Product.Product, Guid>, 
      IProductRepository
{
    public EfProductRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
