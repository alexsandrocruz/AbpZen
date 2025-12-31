using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Chat.Conversations;

public interface IConversationRepository : IBasicRepository<Conversation, Guid>
{
    Task<ConversationPair> FindPairAsync(Guid senderId, Guid targetId, CancellationToken cancellationToken = default);

    Task<List<ConversationWithTargetUser>> GetListByUserIdAsync(Guid userId, string filter, CancellationToken cancellationToken = default);

    Task<int> GetTotalUnreadMessageCountAsync(Guid userId, CancellationToken cancellationToken = default);
}
