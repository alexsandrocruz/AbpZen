using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Product;

public class EfProductRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, Product, Guid>, 
      IProductRepository
{
    public EfProductRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
