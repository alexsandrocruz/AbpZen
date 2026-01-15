using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Contract;

public class EfContractRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Contract.Contract, Guid>, 
      IContractRepository
{
    public EfContractRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
