using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Chat.Messages;

public interface IUserMessageRepository : IBasicRepository<UserMessage, Guid>
{
    Task<List<MessageWithDetails>> GetMessagesAsync(Guid userId, Guid targetUserId, int skipCount, int maxResultCount, CancellationToken cancellationToken = default);
    
    Task<bool> HasConversationAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default);

    Task<List<UserMessage>> GetListAsync(Guid messageId, CancellationToken cancellationToken = default);
    
    Task<MessageWithDetails> GetLastMessageAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default);

    Task<List<Guid>> GetAllMessageIdsAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default);

    Task DeleteAllMessages(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default);
}
