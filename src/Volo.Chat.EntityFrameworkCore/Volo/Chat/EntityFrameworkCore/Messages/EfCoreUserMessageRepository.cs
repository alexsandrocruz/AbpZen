using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Chat.Messages;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Volo.Chat.EntityFrameworkCore.Messages;

public class EfCoreUserMessageRepository : EfCoreRepository<IChatDbContext, UserMessage, Guid>, IUserMessageRepository
{
    public EfCoreUserMessageRepository(IDbContextProvider<IChatDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<MessageWithDetails>> GetMessagesAsync(Guid userId, Guid targetUserId, int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
    {
        var query = from chatUserMessage in (await GetDbSetAsync())
                    join message in (await GetDbContextAsync()).ChatMessages on chatUserMessage.ChatMessageId equals message.Id
                    where userId == chatUserMessage.UserId && targetUserId == chatUserMessage.TargetUserId
                    select new MessageWithDetails
                    {
                        UserMessage = chatUserMessage,
                        Message = message
                    };

        return await query.OrderByDescending(x => x.Message.CreationTime).PageBy(skipCount, maxResultCount).ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public virtual async Task<MessageWithDetails> GetLastMessageAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var query = from chatUserMessage in (await GetDbSetAsync())
                    join message in (await GetDbContextAsync()).ChatMessages on chatUserMessage.ChatMessageId equals message.Id
                    where userId == chatUserMessage.UserId && targetUserId == chatUserMessage.TargetUserId
                    select new MessageWithDetails
                    {
                        UserMessage = chatUserMessage,
                        Message = message
                    };

        return await query.OrderByDescending(x => x.Message.CreationTime).FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
    }

    public virtual async Task<bool> HasConversationAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).AnyAsync(p => p.UserId == userId && p.TargetUserId == targetUserId, cancellationToken);
    }

    public async Task<List<UserMessage>> GetListAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(message => message.ChatMessageId == messageId).ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task<List<Guid>> GetAllMessageIdsAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetDbSetAsync()).Where(message =>
                (message.UserId == userId && message.TargetUserId == targetUserId) ||
                (message.UserId == targetUserId && message.TargetUserId == userId))
            .Select(message => message.ChatMessageId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }
    
    public async Task DeleteAllMessages(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        await (await GetDbSetAsync()).Where(message =>
            (message.UserId == userId && message.TargetUserId == targetUserId) || 
            (message.UserId == targetUserId && message.TargetUserId == userId))
            .ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }
}
