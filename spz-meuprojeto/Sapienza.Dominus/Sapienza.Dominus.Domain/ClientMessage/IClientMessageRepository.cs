using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientMessage;

public interface IClientMessageRepository : IRepository<Dominus.ClientMessage.ClientMessage, Guid>
{
}
