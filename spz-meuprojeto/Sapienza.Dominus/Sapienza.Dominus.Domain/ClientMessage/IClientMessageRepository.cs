using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ClientMessage;

public interface IClientMessageRepository : IRepository<Sapienza.Dominus.ClientMessage.ClientMessage, Guid>
{
}
