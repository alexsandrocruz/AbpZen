using System;
using Sapienza.Dominus.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Sapienza.Dominus.Transaction;

public class EfTransactionRepository 
    : EfCoreRepository<Sapienza.DominusDbContext, Sapienza.Dominus.Transaction.Transaction, Guid>, 
      ITransactionRepository
{
    public EfTransactionRepository(IDbContextProvider<Sapienza.DominusDbContext> dbContextProvider) 
        : base(dbContextProvider)
    {
    }
}
