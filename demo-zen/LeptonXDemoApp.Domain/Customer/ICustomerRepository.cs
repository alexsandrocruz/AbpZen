using System;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Customer;

public interface ICustomerRepository : IRepository<LeptonXDemoApp.Customer.Customer, Guid>
{
}
