using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Product;

public class EfProductRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Product.Product, Guid>, 
      IProductRepository
{
    public EfProductRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
