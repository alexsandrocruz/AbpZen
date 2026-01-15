using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Client;

public interface IClientRepository : IRepository<Dominus.Client.Client, Guid>
{
}
