using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Chat.Conversations;
using Volo.Chat.Messages;
using Volo.Chat.Users;

namespace Volo.Chat.MongoDB.Messages;

public class MongoConversationRepository : MongoDbRepository<IChatMongoDbContext, Conversation, Guid>, IConversationRepository
{
    public MongoConversationRepository(IMongoDbContextProvider<IChatMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public virtual async Task<ConversationPair> FindPairAsync(Guid senderId, Guid targetId, CancellationToken cancellationToken = default)
    {
        var matchedConversations = await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => (x.UserId == senderId && x.TargetUserId == targetId) ||
                        (x.UserId == targetId && x.TargetUserId == senderId)).ToListAsync(GetCancellationToken(cancellationToken));

        if (!matchedConversations.Any())
        {
            return null;
        }

        return new ConversationPair
        {
            SenderConversation = matchedConversations.SingleOrDefault(x => x.UserId == senderId),
            TargetConversation = matchedConversations.SingleOrDefault(x => x.UserId == targetId)
        };
    }

    public virtual async Task<List<ConversationWithTargetUser>> GetListByUserIdAsync(Guid userId, string filter, CancellationToken cancellationToken = default)
    {
        cancellationToken = GetCancellationToken(cancellationToken);
        var userQuery =
            (from chatConversation in (await GetMongoQueryableAsync(cancellationToken))
             join targetUser in (await GetDbContextAsync(cancellationToken)).ChatUsers on chatConversation
                     .TargetUserId equals targetUser.Id
             where userId == chatConversation.UserId && (filter == null || filter == "" || (targetUser.Name.Contains(filter) || targetUser.Surname.Contains(filter) || targetUser.UserName.Contains(filter)))
             orderby chatConversation.LastMessageDate descending
             select targetUser);

        var conversationsWithTargetDetails = await ApplyDataFilters<IMongoQueryable<ChatUser>, ChatUser>(userQuery).Select(targetUser =>
            new ConversationWithTargetUser
            {
                TargetUser = targetUser
            }).ToListAsync(cancellationToken: cancellationToken);

        var conversations = await (await GetMongoQueryableAsync(cancellationToken)).Where(x => x.UserId == userId).ToListAsync(cancellationToken);

        foreach (var conversationWithTargetDetails in conversationsWithTargetDetails)
        {
            conversationWithTargetDetails.Conversation =
                conversations.Single(x => x.TargetUserId == conversationWithTargetDetails.TargetUser.Id);
        }

        return conversationsWithTargetDetails;
    }

    public virtual async Task<int> GetTotalUnreadMessageCountAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (await GetMongoQueryableAsync(cancellationToken))
            .Where(x => x.UserId == userId && x.LastMessageSide == ChatMessageSide.Receiver)
            .SumAsync(x => x.UnreadMessageCount, cancellationToken: GetCancellationToken(cancellationToken));
    }
}
