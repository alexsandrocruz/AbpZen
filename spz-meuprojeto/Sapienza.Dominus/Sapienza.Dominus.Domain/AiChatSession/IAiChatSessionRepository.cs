using System;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.AiChatSession;

public interface IAiChatSessionRepository : IRepository<Dominus.AiChatSession.AiChatSession, Guid>
{
}
