using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Contract;

public class EfContractRepository 
    : EfCoreRepository<DominusDbContext, Dominus.Contract.Contract, Guid>, 
      IContractRepository
{
    public EfContractRepository(IDbContextProvider<DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
