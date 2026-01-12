using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.OrderItem;

public interface IOrderItemRepository : IRepository<LeptonXDemoApp.OrderItem.OrderItem, Guid>
{
}
