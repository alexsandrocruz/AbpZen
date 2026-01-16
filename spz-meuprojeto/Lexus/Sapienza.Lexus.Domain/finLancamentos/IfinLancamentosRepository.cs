using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finLancamentos;

public interface IfinLancamentosRepository : IRepository<Sapienza.Lexus.finLancamentos.finLancamentos, Guid>
{
}
