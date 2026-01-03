using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.OrderItem;

public class EfOrderItemRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.OrderItem.OrderItem, Guid>, 
      IOrderItemRepository
{
    public EfOrderItemRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
