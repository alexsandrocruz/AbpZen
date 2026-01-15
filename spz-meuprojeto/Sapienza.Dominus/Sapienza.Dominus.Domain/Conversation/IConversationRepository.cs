using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Conversation;

public interface IConversationRepository : IRepository<Dominus.Conversation.Conversation, Guid>
{
}
