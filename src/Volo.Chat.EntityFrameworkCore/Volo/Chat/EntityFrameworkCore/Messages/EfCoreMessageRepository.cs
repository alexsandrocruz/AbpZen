using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Chat.Messages;

namespace Volo.Chat.EntityFrameworkCore.Messages;

public class EfCoreMessageRepository : EfCoreRepository<IChatDbContext, Message, Guid>, IMessageRepository
{
    public EfCoreMessageRepository(IDbContextProvider<IChatDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task DeleteALlMessagesAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
         await (await GetDbSetAsync()).Where(message => ids.Contains(message.Id)).ExecuteDeleteAsync(GetCancellationToken(cancellationToken));
    }
}
