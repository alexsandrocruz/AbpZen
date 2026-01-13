using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finRecibos;

public interface IfinRecibosRepository : IRepository<Sapienza.Lexus.finRecibos.finRecibos, Guid>
{
}
