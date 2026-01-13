using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuPermissoes;

public interface IusuPermissoesRepository : IRepository<Sapienza.Lexus.usuPermissoes.usuPermissoes, Guid>
{
}
