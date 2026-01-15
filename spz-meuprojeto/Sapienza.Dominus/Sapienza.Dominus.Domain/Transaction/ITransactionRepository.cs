using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Transaction;

public interface ITransactionRepository : IRepository<Sapienza.Dominus.Transaction.Transaction, Guid>
{
}
