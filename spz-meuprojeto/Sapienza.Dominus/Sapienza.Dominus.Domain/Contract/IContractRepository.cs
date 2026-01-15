using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Contract;

public interface IContractRepository : IRepository<Dominus.Contract.Contract, Guid>
{
}
