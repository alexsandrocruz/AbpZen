using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Contract;

public interface IContractRepository : IRepository<Sapienza.Dominus.Contract.Contract, Guid>
{
}
