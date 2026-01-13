using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.EventoZen.Client;

public interface IClientRepository : IRepository<Sapienza.EventoZen.Client.Client, Guid>
{
}
