using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finPrestacaoContas;

public interface IfinPrestacaoContasRepository : IRepository<Sapienza.Lexus.finPrestacaoContas.finPrestacaoContas, Guid>
{
}
