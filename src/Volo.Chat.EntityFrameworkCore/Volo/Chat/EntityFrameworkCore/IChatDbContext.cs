using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Chat.Conversations;
using Volo.Chat.Messages;
using Volo.Chat.Users;

namespace Volo.Chat.EntityFrameworkCore;

[ConnectionStringName(ChatDbProperties.ConnectionStringName)]
public interface IChatDbContext : IEfCoreDbContext
{
    DbSet<Message> ChatMessages { get; }

    DbSet<ChatUser> ChatUsers { get; }

    DbSet<UserMessage> ChatUserMessages { get; }

    DbSet<Conversation> ChatConversations { get; }
}
