#nullable enable
using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Client;

public interface IClientRepository : IRepository<Sapienza.Lexus.Client.Client, Guid>
{
}
