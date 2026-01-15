using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatMessage;

public interface IAiChatMessageRepository : IRepository<Sapienza.Dominus.AiChatMessage.AiChatMessage, Guid>
{
}
