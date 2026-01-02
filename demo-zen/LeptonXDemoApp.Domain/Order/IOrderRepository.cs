using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Order;

public interface IOrderRepository : IRepository<LeptonXDemoApp.Order.Order, Guid>
{
}
