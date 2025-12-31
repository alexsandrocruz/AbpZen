using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Chat.Messages;

namespace Volo.Chat.MongoDB.Messages;

public class MongoUserMessageRepository : MongoDbRepository<IChatMongoDbContext, UserMessage, Guid>, IUserMessageRepository
{
    public MongoUserMessageRepository(IMongoDbContextProvider<IChatMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<List<MessageWithDetails>> GetMessagesAsync(Guid userId, Guid targetUserId, int skipCount, int maxResultCount,
        CancellationToken cancellationToken = default)
    {
        var messageListQuery = ApplyDataFilters<IMongoQueryable<Message>, Message>(from chatUserMessage in await GetMongoQueryableAsync(cancellationToken)
                                                                                   join message in (await GetDbContextAsync(cancellationToken)).ChatMessages on chatUserMessage.ChatMessageId equals message.Id
                                                                                   where userId == chatUserMessage.UserId && targetUserId == chatUserMessage.TargetUserId
                                                                                   select message).Select(message => new MessageWithDetails
                                                                                   {
                                                                                       Message = message
                                                                                   });

        var userMessageListQuery = (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.UserId == userId && x.TargetUserId == targetUserId);

        var chatMessageWithDetailsList = await messageListQuery.OrderByDescending(x => x.Message.CreationTime)
            .PageBy<MessageWithDetails, IMongoQueryable<MessageWithDetails>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var userMessagesList = await userMessageListQuery
            .PageBy<UserMessage, IMongoQueryable<UserMessage>>(skipCount, maxResultCount)
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var chatMessageWithDetails in chatMessageWithDetailsList)
        {
            chatMessageWithDetails.UserMessage =
                userMessagesList.Find(x => x.ChatMessageId == chatMessageWithDetails.Message.Id);
        }

        return chatMessageWithDetailsList;
    }

    public async Task<MessageWithDetails> GetLastMessageAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var messageListQuery = ApplyDataFilters<IMongoQueryable<Message>, Message>(from chatUserMessage in await GetMongoQueryableAsync(cancellationToken)
                                                                                   join message in (await GetDbContextAsync(cancellationToken)).ChatMessages on chatUserMessage.ChatMessageId equals message.Id
                                                                                   where userId == chatUserMessage.UserId && targetUserId == chatUserMessage.TargetUserId
                                                                                   select message).Select(message => new MessageWithDetails
                                                                                   {
                                                                                       Message = message
                                                                                   });

        var userMessageListQuery = (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.UserId == userId && x.TargetUserId == targetUserId);

        var chatMessageWithDetailsList = await messageListQuery.OrderByDescending(x => x.Message.CreationTime)
            .PageBy<MessageWithDetails, IMongoQueryable<MessageWithDetails>>(0, 1)
            .ToListAsync(GetCancellationToken(cancellationToken));

        var userMessagesList = await userMessageListQuery
            .PageBy<UserMessage, IMongoQueryable<UserMessage>>(0, 1)
            .ToListAsync(GetCancellationToken(cancellationToken));

        foreach (var chatMessageWithDetails in chatMessageWithDetailsList)
        {
            chatMessageWithDetails.UserMessage =
                userMessagesList.Find(x => x.ChatMessageId == chatMessageWithDetails.Message.Id);
        }

        return chatMessageWithDetailsList.FirstOrDefault();
    }

    public async Task<List<Guid>> GetAllMessageIdsAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(message => (message.UserId == userId && message.TargetUserId == targetUserId) ||
                              (message.UserId == targetUserId && message.TargetUserId == userId))
            .Select(message => message.ChatMessageId)
            .ToListAsync(GetCancellationToken(cancellationToken));
    }

    public async Task DeleteAllMessages(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        var ids = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(message => (message.UserId == userId && message.TargetUserId == targetUserId) ||
                              (message.UserId == targetUserId && message.TargetUserId == userId))
            .Select(message => message.Id)
            .ToListAsync(GetCancellationToken(cancellationToken));

        await DeleteManyAsync(ids, cancellationToken: cancellationToken);
    }

    public virtual async Task<bool> HasConversationAsync(Guid userId, Guid targetUserId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).AnyAsync(p => p.UserId == userId && p.TargetUserId == targetUserId);
    }

    public async Task<List<UserMessage>> GetListAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken)).Where(message => message.ChatMessageId == messageId).ToListAsync(GetCancellationToken(cancellationToken));
    }
}
