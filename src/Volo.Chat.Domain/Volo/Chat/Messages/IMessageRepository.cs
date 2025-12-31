using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Volo.Chat.Messages;

public interface IMessageRepository : IBasicRepository<Message, Guid>
{
    Task DeleteALlMessagesAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
}
