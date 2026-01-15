using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Client;

public interface IClientRepository : IRepository<Sapienza.Dominus.Client.Client, Guid>
{
}
