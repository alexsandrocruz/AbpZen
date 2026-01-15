using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ChatMessage;

public interface IChatMessageRepository : IRepository<Dominus.ChatMessage.ChatMessage, Guid>
{
}
