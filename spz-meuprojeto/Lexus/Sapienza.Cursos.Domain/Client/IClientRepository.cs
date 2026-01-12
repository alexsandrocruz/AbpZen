using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Client;

public interface IClientRepository : IRepository<Sapienza.Cursos.Client.Client, Guid>
{
}
