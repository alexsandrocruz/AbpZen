using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Chat.Messages;

namespace Volo.Chat.MongoDB.Messages;

public class MongoMessageRepository : MongoDbRepository<IChatMongoDbContext, Message, Guid>, IMessageRepository
{
    public MongoMessageRepository(IMongoDbContextProvider<IChatMongoDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task DeleteALlMessagesAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        await DeleteManyAsync(ids, cancellationToken: cancellationToken);
    }
}
