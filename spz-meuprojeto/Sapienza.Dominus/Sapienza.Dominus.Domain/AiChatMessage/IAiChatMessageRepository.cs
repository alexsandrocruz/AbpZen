using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatMessage;

public interface IAiChatMessageRepository : IRepository<Dominus.AiChatMessage.AiChatMessage, Guid>
{
}
