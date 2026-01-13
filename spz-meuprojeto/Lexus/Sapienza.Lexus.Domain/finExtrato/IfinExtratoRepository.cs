using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finExtrato;

public interface IfinExtratoRepository : IRepository<Sapienza.Lexus.finExtrato.finExtrato, Guid>
{
}
