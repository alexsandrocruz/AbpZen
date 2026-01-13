using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finContas;

public interface IfinContasRepository : IRepository<Sapienza.Lexus.finContas.finContas, Guid>
{
}
