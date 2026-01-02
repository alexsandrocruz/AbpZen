using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Order;

public class EfOrderRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.Order.Order, Guid>, 
      IOrderRepository
{
    public EfOrderRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
