using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Conversation;

public interface IConversationRepository : IRepository<Sapienza.Dominus.Conversation.Conversation, Guid>
{
}
