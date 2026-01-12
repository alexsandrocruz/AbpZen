using System;
using LeptonXDemoApp.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace LeptonXDemoApp.Customer;

public class EfCustomerRepository 
    : EfCoreRepository<LeptonXDemoAppDbContext, LeptonXDemoApp.Customer.Customer, Guid>, 
      ICustomerRepository
{
    public EfCustomerRepository(IDbContextProvider<LeptonXDemoAppDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
